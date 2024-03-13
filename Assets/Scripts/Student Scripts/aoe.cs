using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class aoe : MonoBehaviour
{
    private int bulletDamage;
    private bool healer;
    public GameObject student;
    public float lingerTime = 0.4f;

    void Awake() {
        StudentInfo studentScript = student.GetComponent<StudentInfo>();
        bulletDamage = studentScript.damage;
        healer = studentScript.healer;
        StartCoroutine(DestroyAfterDelay(lingerTime));
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (healer)
        {
            // Give health to allies
            other.gameObject.GetComponent<StudentInfo>().Heal(bulletDamage);
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
