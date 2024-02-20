using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int hp = 10;
    public int spawnPointIndex = 0;
    public int damage = 5;
    public float speed = 2f;
    public float attackRadius = 0.55f;


    // Calculating damage done to enemy
    public void takeDamage(int dmg){
        hp -= dmg;

        if (hp <= 0){
            EnemyDeath();
        }
    } 

    public void EnemyDeath() {
        Destroy(gameObject);
    }
}
