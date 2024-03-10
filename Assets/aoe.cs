using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class aoe : MonoBehaviour
{
    private int bulletDamage;
    private bool healer;
    public GameObject student;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Gets needed data for damage
        StudentInfo studentScript = student.GetComponent<StudentInfo>();
        bulletDamage = studentScript.damage;
        healer = studentScript.healer;

        if (healer)
        {
            // Give health to allies
            other.gameObject.GetComponent<StudentInfo>().Heal(bulletDamage);  
            Destroy(gameObject);  
        } 
        else 
        {
            // Deal damage
            other.gameObject.GetComponent<EnemyInfo>().takeDamage(bulletDamage);
            Destroy(gameObject);
        }

    }
}
