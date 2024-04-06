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
    protected int bulletDamage;
    private float bulletDistance;
    private Vector2 origin;

    private Transform target;

    private Vector2 direction;

    public AudioClip sfx;

    public void SetAttributes(float speed, int damage, float distance, Vector2 studentPos)
    {
        bulletSpeed = speed;
        bulletDamage = damage;
        bulletDistance = distance;
        origin = studentPos;
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }
    private void FixedUpdate()
    {
        if (target)
        {
            direction = (target.position - transform.position).normalized;
        }

        if (direction == Vector2.zero && !target) {
            // sometimes bullets are fired as the student dies, and they don't move
            Destroy(gameObject);
        }

        rb.velocity = direction * bulletSpeed;

        Vector2 delta = (Vector2) transform.position - origin;
        if (delta.magnitude > bulletDistance)
        {
            // destroy the bullet when it gets too far from the student
            Destroy(gameObject);
        }
    }
}
