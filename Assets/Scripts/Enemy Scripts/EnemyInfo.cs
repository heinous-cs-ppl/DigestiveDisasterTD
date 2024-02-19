using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int totalHealth;
    private int currentHealth;
    public int spawnPointIndex;
    public int damage;
    public float speed;


    // Calculating damage done to enemy
    public void takeDamage(int dmg){
        currentHealth -= dmg;

        if (currentHealth <= 0){
            Destroy(gameObject);
        }
    } 
}
