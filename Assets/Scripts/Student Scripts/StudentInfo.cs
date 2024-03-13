using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentInfo : MonoBehaviour
{
    public int cost = 100;
    [Header("Student Attributes")]
    public int maxHp = 10;
    [SerializeField] private Bar healthBar;
    [SerializeField] private Bar purifyTimerBar;
    public int damage = 1;
    public float range = 3f;
    public float rotationSpeed = 1000f;
    [Header("Bullet Attributes")]
    public float bps = 1f; // bullets per second
    public float bulletSpeed = 5f;
    public bool healer = false;
    public float bulletDistance = 3f;

    public string description = "SAMPLE TEXT";


    // extra fields to hold the original values of the attributes
    private int originalDamage;
    private float originalRange;
    private float originalBps;
    private float originalBulletSpeed;
    private float originalBulletDistance;

    private float buffTime = 10f;
    [HideInInspector] public bool buffed = false;

    public Turret turret;

    private Coroutine timerOn;

    [Header("Materials for outline")]
    public Material outline;
    public Material noOutline;

    [Header("Sprite stuff")]
    public float offsetX = 0;
    private void Start()
    {
        healthBar.setMaxValue(maxHp);
        healthBar.setValue(maxHp);

        purifyTimerBar.setMaxValue(buffTime);
        purifyTimerBar.gameObject.SetActive(false);

        originalDamage = damage;
        originalRange = range;
        originalBps = bps;
        originalBulletSpeed = bulletSpeed;
        originalBulletDistance = bulletDistance;
    }
    // Getter for student health
    public int getHealth()
    {
        return healthBar.getValue();
    }

    public void TakeDamage(int dmg)
    {
        int currentHp = getHealth();
        int newHp = currentHp - dmg;

        if (newHp <= 0)
        {
            StudentDeath();
        }

        healthBar.setValue(newHp);

        if (StudentManager.selected == gameObject) UIManager.UpdateSelectedBars(this);
    }

    public void Heal(int heal)
    {
        int currentHp = getHealth();

        // Heals the target, but will first check if overhealed. If it is, give max hp, if not, heal normal amount
        if (currentHp + heal > maxHp)
        {
            healthBar.setValue(maxHp);
        }
        else
        {
            healthBar.setValue(currentHp + heal);
        }

        if (StudentManager.selected == gameObject) UIManager.UpdateSelectedBars(this);
    }
    public void StudentDeath()
    {
        RaycastHit2D plothit = Physics2D.Raycast(transform.position, Vector2.zero, 1f, LevelManager.instance.plotLayer);
        Plot plot = plothit.transform.gameObject.GetComponent<Plot>();
        if (StudentManager.selected == gameObject) StudentManager.Deselect();
        Destroy(gameObject);
        plot.student = null;

        LevelManager.instance.studentsDead += 1;
        UIManager.UpdateStudentsDeadText();
        if (LevelManager.instance.studentsDead >= LevelManager.instance.deathLimit) {
            LevelManager.instance.GameOver();
        }
    }

    // handles buffs when student is fed purified food
    public void Feed()
    {
        // heal the student for 30% of their max hp (rounded up because I'm generous)
        int toHeal = Mathf.CeilToInt(maxHp * 0.3f);
        Heal(toHeal);

        if (buffed)
        {
            RefreshBuff();
            return;
        }

        buffed = true;
        // increase bps, damage, range, bullet speed, bullet lifetime by 30%
        bps *= 1.3f;
        damage = (int)Mathf.Ceil(damage * 1.3f);
        range *= 1.3f;
        bulletSpeed *= 1.3f;
        bulletDistance *= 1.3f;

        

        // Vacuous students don't have a turret
        if (turret != null)
        {
            purifyTimerBar.gameObject.SetActive(true);
            purifyTimerBar.setValue(buffTime);

            turret.SetStudentAttributes(this);
            turret.SetBulletAttributes(this);
            // delay
            timerOn = StartCoroutine(BuffTimer());
        }
        UIManager.UpdateSelectedBars(this);
        Debug.Log("Buffed student");
    }

    private IEnumerator BuffTimer()
    {
        // wait for timer before removing buffs
        float iterationTime = 0.01f;
        float counter = 0f;
        while (counter < buffTime) {
            yield return new WaitForSeconds(iterationTime);
            counter += iterationTime;
            purifyTimerBar.setValue(buffTime - counter);
        }
        
        RevertBuffs();
    }

    private void RefreshBuff()
    {
        // stop the current timer
        if (timerOn != null) StopCoroutine(timerOn);

        Debug.Log("refreshed buff");
        purifyTimerBar.setValue(buffTime);

        // start a new timer
        timerOn = StartCoroutine(BuffTimer());
    }

    private void RevertBuffs()
    {
        damage = originalDamage;
        bps = originalBps;
        range = originalRange;
        bulletSpeed = originalBulletSpeed;
        bulletDistance = originalBulletDistance;
        buffed = false;

        if (turret != null)
        {
            turret.SetStudentAttributes(this);
            turret.SetBulletAttributes(this);
        }

        Debug.Log("Removed buffs");
        purifyTimerBar.gameObject.SetActive(false);

        // reselect student if it is currently selected
        if (StudentManager.selected == gameObject) StudentManager.Select(gameObject);
    }
}
