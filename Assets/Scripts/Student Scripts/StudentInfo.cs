using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentInfo : MonoBehaviour
{
    public int cost = 100;
    [Header("Student Attributes")]
    public int maxHp = 10;
    [HideInInspector] public int currentHp;
    public int damage = 1;
    public float range = 3f;
    public float rotationSpeed = 1000f;
    [Header("Bullet Attributes")]
    public float bps = 1f; // bullets per second
    public float bulletSpeed = 5f;
    public float bulletLifetime = 1f;


    // extra fields to hold the original values of the attributes
    private int originalDamage;
    private float originalRange;
    private float originalBps;
    private float originalBulletSpeed;
    private float originalBulletLifetime;

    private float buffTime = 10f;
    [HideInInspector] public bool buffed = false;

    public Turret turret;

    private Coroutine timerOn;
    private void Start() {
        currentHp = maxHp;

        originalDamage = damage;
        originalRange = range;
        originalBps = bps;
        originalBulletSpeed = bulletSpeed;
        originalBulletLifetime = bulletLifetime;
    }

    public void TakeDamage(int dmg) {
        currentHp -= dmg;
        if(currentHp <= 0) {
            StudentDeath();
        }
    }

    public void StudentDeath() {
        Destroy(gameObject);
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

        turret.SetStudentAttributes(this);
        turret.SetBulletAttributes(this);
        Debug.Log("Buffed student");
        // delay
        timerOn = StartCoroutine(BuffTimer());
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

        turret.SetStudentAttributes(this);
        turret.SetBulletAttributes(this);

        Debug.Log("Removed buffs");
    }
}
