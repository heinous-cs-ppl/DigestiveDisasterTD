using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class aoe : MonoBehaviour
{
    private int bulletDamage;
    private bool healer;
    private bool flask;
    private bool slow;
    [HideInInspector] public float delay; 
    [HideInInspector] public float slowPercentage;
    public GameObject student;
    public float lingerTime = 0.4f;

    void Awake() 
    {
        // To set this, make sure you check mark the box in the editor in the student info script on the tower. 
        StudentInfo studentScript = student.GetComponent<StudentInfo>();
        bulletDamage = studentScript.damage;
        
        // Sets true if the turret we are dealing with is a healing turret 
        healer = studentScript.healer;

        // Checks if student is a purify tower
        flask = studentScript.purify;

        // Checks if turret can slow 
        slow = studentScript.slow;
    

        StartCoroutine(DestroyAfterDelay(lingerTime));
    
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (healer)
        {
            // Give health to allies
            other.gameObject.GetComponent<StudentInfo>().Heal(bulletDamage);
        } 
        else if (flask)
        {
            // Dealing purified damage
            other.gameObject.GetComponent<EnemyInfo>().takePurifyDamage(bulletDamage);
        }
        else if(slow)
        {            
            Debug.Log("aoeScript: " + slowPercentage);
            // Slows enemies
            other.gameObject.GetComponent<PathFollow>().slowSpeed(slowPercentage, delay);
        }
        else 
        {
            // Deal damage
            other.gameObject.GetComponent<EnemyInfo>().takeDamage(bulletDamage);
        }
    }

    IEnumerator DestroyAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
