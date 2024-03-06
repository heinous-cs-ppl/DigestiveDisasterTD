using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentInfo : MonoBehaviour
{
    public int cost = 100;
    [Header("Student Attributes")]
    public int maxHp = 10;
     public int currentHp;
    public int damage = 1;
    public float range = 3f;
    public float rotationSpeed = 1000f;
    [Header("Bullet Attributes")]
    public float bps = 1f; // bullets per second
    public float bulletSpeed = 5f;
    public float bulletLifetime = 1f;
    public HealthBar healthBar;


    // extra fields to hold the original values of the attributes
    private int originalDamage;
    private float originalRange;
    private float originalBps;
    private float originalBulletSpeed;
    private float originalBulletLifetime;

    private float buffTime = 10f;
    public bool buffed = false;

    public Turret turret;

    private Coroutine timerOn;
    private void Start() {
        currentHp = maxHp;
        healthBar.setMaxHealth(maxHp);

        originalDamage = damage;
        originalRange = range;
        originalBps = bps;
        originalBulletSpeed = bulletSpeed;
        originalBulletLifetime = bulletLifetime;
    }

    public void TakeDamage(int dmg) {
        int newHp = currentHp - dmg;
        currentHp = newHp;
        healthBar.setHealth(newHp);
        if(currentHp <= 0) {
            StudentDeath();
        }
    }

    public void Heal(int heal){
        // Heals the target, but will first check if overhealed. If it is, give max hp, if not, heal normal amount
        if (currentHp + heal > maxHp ) {
            currentHp = maxHp;
            healthBar.setHealth(maxHp);
        }  
        else 
        {
            int newHp = currentHp + heal;
            currentHp = newHp;
            healthBar.setHealth(newHp);
        }
    }
    public void StudentDeath() {
        RaycastHit2D plothit = Physics2D.Raycast(transform.position, Vector2.zero, 1f, LevelManager.instance.plotLayer);
        Plot plot = plothit.transform.gameObject.GetComponent<Plot>();
        Destroy(gameObject);
        plot.student = null;
    }

    // handles buffs when student is fed purified food
    public void Feed() {
        // heal the student for 30% of their max hp (rounded up because I'm generous)
        currentHp = (int) (currentHp + Mathf.Ceil(maxHp * 0.3f));
        // don't allow the hp to exceed the max
        if(currentHp > maxHp) currentHp = maxHp;

        if (buffed) {
            RefreshBuff();
            return;
        }

        buffed = true;
        // increase bps, damage, range, bullet speed, bullet lifetime by 30%
        bps *= 1.3f;
        damage = (int) Mathf.Ceil(damage * 1.3f);
        range *= 1.3f;
        bulletSpeed *= 1.3f;
        bulletLifetime *= 1.3f;

        // Vacuous students don't have a turret
        if (turret != null) {
            turret.SetStudentAttributes(this);
            turret.SetBulletAttributes(this);
            // delay
            timerOn = StartCoroutine(BuffTimer());
        }
        Debug.Log("Buffed student");
    }

    private IEnumerator BuffTimer() {
        // wait for timer before removing buffs
        yield return new WaitForSeconds(buffTime);
        RevertBuffs();
    }

    private void RefreshBuff()
    {
        // stop the current timer
        if (timerOn != null) StopCoroutine(timerOn);

        Debug.Log("refreshed buff");
        // start a new timer
        timerOn = StartCoroutine(BuffTimer());
    }

    private void RevertBuffs() {
        damage = originalDamage;
        bps = originalBps;
        range = originalRange;
        bulletSpeed = originalBulletSpeed;
        bulletLifetime = originalBulletLifetime;
        buffed = false;

        if (turret != null) {
            turret.SetStudentAttributes(this);
            turret.SetBulletAttributes(this);
        }

        Debug.Log("Removed buffs");

        // reselect student if it is currently selected
        if (StudentManager.selected == gameObject) StudentManager.Select(gameObject);
    }
}
