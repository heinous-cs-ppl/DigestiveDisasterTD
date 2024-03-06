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
    // Start is called before the first frame update
    void Start()
    {
        EnemyInfo enemy = GetComponent<EnemyInfo>();
        damage = enemy.damage;
        triggerRadius = enemy.attackRadius;
        damageRadius = triggerRadius * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        // get an array of enemies in the range of the food
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
            // destroy the enemy
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {

        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, triggerRadius);

    }


}
