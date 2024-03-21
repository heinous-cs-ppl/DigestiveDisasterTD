using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class MachineStudent : Turret {
    private bool canAttack = true;
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
                if(!target.gameObject.GetComponent<EnemyInfo>().isBoss) {
                    // if not attacking the boss
                    Attack(target.gameObject);
                    target = null;
                } else {
                    // if the target is the boss
                    // while the boss is in range, deal some dps
                    AttackBoss(target.gameObject);
                    target = null;
                    Debug.Log("Attacking boss");
                }
                
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
        float cooldown = 0.75f + 0.005f * (maxPurify - currentPurify);
        
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

    private void AttackBoss(GameObject boss) {
        // start the attack animation
        // call coroutine to deal dps (using purifyDamage in EnemyInfo), checking constantly if the boss is still in range
        // once the boss is out of range, stop the attack animation
        canAttack = false;
        anim.SetTrigger("Attack");
        StartCoroutine(DamageBoss(boss));
    }

    private IEnumerator DamageBoss(GameObject boss) {
        StudentInfo info = gameObject.GetComponent<StudentInfo>();
        float iterationTime = 1/info.bps;
        float range = info.range;
        // do dps based on damage
        // check if the boss is in range of the student
        while (boss != null && BossInRange(boss.GetComponent<Collider2D>(), range))
        {
            boss.GetComponent<EnemyInfo>().takePurifyDamage(info.damage);
            yield return new WaitForSeconds(iterationTime);
            range = info.range;      // to check if the student gets its range buffed by food
            Debug.Log("Damaged boss");
        }
        anim.SetTrigger("AttackEnd");

        // small delay before attack is allowed
        yield return new WaitForSeconds(0.1f);

        // set canAttack to true
        canAttack = true;
    }

    bool BossInRange(Collider2D bossCollider, float range) {
        // Check if any colliders are within the specified range of this GameObject
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);

        // Loop through all colliders found
        foreach (Collider2D collider in colliders)
        {
            // Check if the collider found is the one we're looking for
            if (collider == bossCollider)
            {
                return true; // Collider is within range
            }
        }

        return false; // Collider is not within range
    }
}