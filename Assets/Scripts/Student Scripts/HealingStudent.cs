using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealingTurret : Turret
{
    private new void Update()
    {
        if(ShouldFire()){
            FindTarget();
            base.Update();
        }
    }
    private  new void FindTarget()
    {
        // Get all colliders within targeting range that belong to the "Student" layer
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, targetingRange, LayerMask.GetMask("Student"));

        // Initialize variables to store the closest ally and its HP
        Transform closestAlly = null;
        int minHP = int.MaxValue;

        // Iterate through all colliders to find the closest ally with the lowest HP
        foreach (Collider2D collider in colliders)
        {
            // Get the StudentInfo component of the collider's game object
            StudentInfo studentInfo = collider.GetComponent<StudentInfo>();

            // Ensure the student is an ally (not the turret's own student) and has lower HP than current minimum
            if (studentInfo != null && studentInfo != GetComponent<StudentInfo>() && studentInfo.currentHp < minHP)
            {
                closestAlly = collider.transform;
                minHP = studentInfo.currentHp;
            }
        }

        // Set the closest ally as the target
        target = closestAlly;
    }

    private bool ShouldFire()
    {
    // Get all colliders within targeting range that belong to the "Student" layer
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, targetingRange, LayerMask.GetMask("Student"));

    // Check if any ally is not at full health
    foreach (Collider2D collider in colliders)
    {
        StudentInfo studentInfo = collider.GetComponent<StudentInfo>();
        if (studentInfo != null && studentInfo.currentHp < studentInfo.maxHp)
        {
            return true; // Return true if any ally is not at full health
        }
    }

    return false; // Return false if all allies are at full health
}

}