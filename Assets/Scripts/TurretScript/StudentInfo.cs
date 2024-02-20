using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentInfo : MonoBehaviour
{
    [Header("Student Attributes")]
    public int hp = 10;
    public int damage = 1;
    public float range = 3f;
    public float rotationSpeed = 200f;
    [Header("Bullet Attributes")]
    public float bps = 1f; // bullets per second
    public float bulletSpeed = 5f;
    public float bulletLifetime = 1f;

    public void TakeDamage(int dmg) {
        hp -= dmg;
        if(hp <= 0) {
            StudentDeath();
        }
    }

    public void StudentDeath() {
        Destroy(gameObject);
    }
}
