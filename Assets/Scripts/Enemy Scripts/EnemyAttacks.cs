using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class EnemyAttacks : MonoBehaviour
{
    // the amount of damage dealt by the chyme
    private int damage;

    // radius to trigger the enemu attack
    private float triggerRadius;

    // radius in which damage is dealt
    private float damageRadius;

    // layermask to only check for collisions with students
    [SerializeField] private LayerMask studentMask;

    public bool isBoss = false;

    private Coroutine DamageReductionCoroutine;
    [HideInInspector] public int LawyerDamageMultiplier;
    int originalDamage;

    void Start()
    {
        EnemyInfo enemy = GetComponent<EnemyInfo>();
        damage = enemy.damage;
        triggerRadius = enemy.attackRadius;
        damageRadius = triggerRadius * 2f;

        originalDamage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        // get an array of enemies in the range of the food
        if(!isBoss){
            // if the enemy is not a boss
            Collider2D[] studentsInRange = Physics2D.OverlapCircleAll(transform.position, triggerRadius, studentMask);
            if (studentsInRange.Length > 0)
            {
                // there is an enemy in the range of food
                // get an array of students in the damage radius
                Collider2D[] studentsToDamage = Physics2D.OverlapCircleAll(transform.position, damageRadius, studentMask);
                // damage all of the students in the radius
                foreach (Collider2D studentCollision in studentsToDamage)
                {
                    GameObject student = studentCollision.gameObject;
                    student.GetComponent<StudentInfo>().TakeDamage(damage);
                }

                if(Spawner.isBossWave) {
                    Spawner.ReduceBossEnemyCount(gameObject.tag);
                    Debug.Log(gameObject.tag + " self-destruct");
                }
                // destroy the enemy
                Destroy(gameObject);
            }
        }
    }

    public void DamageReduction(float amount, float duration)
    {
        damage = (int) (damage / amount);
        Debug.Log(damage);

        DamageReductionCoroutine = StartCoroutine(NormalDamageAfterDelay(duration));
    }
    IEnumerator NormalDamageAfterDelay(float remainingTime) 
    {
        // Will delay before reseting the attack back to its original speed
        yield return new WaitForSeconds(remainingTime);  
        damage = originalDamage;
        Debug.Log("Back to original damage");   
    }
    public void ResetDebuff()
    {
        damage = originalDamage;
    }

    // private void OnDrawGizmosSelected()
    // {

    //     Handles.color = Color.cyan;
    //     Handles.DrawWireDisc(transform.position, transform.forward, triggerRadius);

    // }


}
