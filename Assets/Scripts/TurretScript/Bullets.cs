using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f; 
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float bulletLifetime = 1;
    private Transform target;

    private float curLifetime = 0;

    private Vector2 direction;

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
