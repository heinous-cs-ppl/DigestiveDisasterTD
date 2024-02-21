using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int hp = 10;
    public int purifyHp = 1;
    public int spawnPointIndex = 0;
    public int damage = 5;
    public float speed = 2f;
    public float attackRadius = 0.55f;

    public GameObject[] buffs;


    // Calculating damage done to enemy
    public void takeDamage(int dmg){
        hp -= dmg;

        if (hp <= 0){
            EnemyDeath();
        }
    } 


    // Calculating purifying damage done to the enemy
    public void purifyDamage(int dmg) {
        purifyHp -= dmg;
        if (purifyHp <= 0) {
            purify();
        }
    }

    public void EnemyDeath() {
        Destroy(gameObject);
    }

    public void purify() {
        // give the player a purified meal
        PurifyManager.GainMeal();
        UIManager.UpdateMealCount();
        Destroy(gameObject);
    }
}
