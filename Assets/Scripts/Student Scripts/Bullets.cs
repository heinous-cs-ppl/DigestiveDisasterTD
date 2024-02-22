using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullets : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    // attributes (defined in StudentInfo.cs)
    private float bulletSpeed; 
    private int bulletDamage;
    private float bulletLifetime;

    private Transform target;

    private float curLifetime = 0;

    private Vector2 direction;

    public void SetAttributes(float speed, int damage, float lifetime) {
        bulletSpeed = speed;
        bulletDamage = damage;
        bulletLifetime = lifetime;
    }
    public void SetTarget(Transform _target){
        target = _target;
    }
    private void FixedUpdate() {
        if (target) {
            direction = (target.position - transform.position).normalized; 
        }

        rb.velocity = direction * bulletSpeed;
        
        curLifetime += Time.fixedDeltaTime;
        if(curLifetime >= bulletLifetime) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
        // Take health from Enemy
        other.gameObject.GetComponent<EnemyInfo>().takeDamage(bulletDamage);
        Destroy(gameObject);
    }
    
}
