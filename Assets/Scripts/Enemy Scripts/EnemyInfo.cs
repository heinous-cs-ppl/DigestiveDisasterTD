using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int health;
    public int spawnPointIndex;
    public int damage;
    public float speed;


    // Calculating damage done to enemy
    public void takeDamage(int dmg){
        health -= dmg;

        if (health <= 0){
            Destroy(gameObject);
        }
    } 
}
