using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class MachineStudent : Turret {
    private bool canAttack = true;
    private StudentInfo student;

    private new void Start() {
        base.Start();
        student = gameObject.GetComponent<StudentInfo>();
    }
    protected new void Update() {
        // Tries to find food to attack
        if (canAttack && target == null)
        {
            FindTarget();
            return;
        }

        // this student doesn't rotate
        else
        {

            // If an enemy is in range, fire at set rate
            timeUntilFire += Time.deltaTime;

            // If the time until fire is larger then 1/bps 
            if (canAttack)
            {
                Attack(target.gameObject);
                target = null;
            }
        }
    }

    private void Attack(GameObject chyme) {
        // set canAttack to false
        canAttack = false;

        // delete chyme after getting its current purify hp and money drop
        EnemyInfo chymousValues = chyme.GetComponent<EnemyInfo>();
        int currentPurify = chymousValues.getPurifyHealth();
        int maxPurify = chymousValues.maxPurifyHp;
        int moneyDrop = chymousValues.moneyDrop;
        Destroy(chyme);
        
        // activate start attack animation
        // attack loop animation will automatically play when start animation is done
        anim.SetTrigger("Attack");

        // once timer hits some value (related to target's purify hp), give a purified meal
        float cooldown;
        if (!student.buffed) {
            cooldown = 0.75f + 0.005f * (maxPurify - currentPurify);
        } else {
            cooldown = 0.75f + 0.005f * (maxPurify - currentPurify) / 0.3f;
        }
        
        StartCoroutine(AttackCooldown(cooldown, moneyDrop));
    }

    private IEnumerator AttackCooldown(float time, int money)
    {
        // let the attack loop animation play until the cooldown resets
        Debug.Log("waiting " + time + " seconds");
        yield return new WaitForSeconds(time);

        // call attack end animation
        anim.SetTrigger("AttackEnd");
        // give player money and purified meal
        PurifyManager.GainMeal();
        UIManager.UpdateMealCount();

        MoneyManager.AddMoney(money);
        UIManager.UpdateMoney();

        // small delay before attack is allowed
        yield return new WaitForSeconds(0.1f);

        // set canAttack to true
        canAttack = true;
    }
}