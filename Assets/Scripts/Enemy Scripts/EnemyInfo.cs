using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int maxHp = 10;
    public int currentHp;
    public HealthBar healthBar;
    public int maxPurifyHp = 1;
    public int currentPurifyHp;
    public int spawnPointIndex = 0;
    public int damage = 5;
    public float speed = 2f;
    public float attackRadius = 0.55f;
    public int moneyDrop = 10;

    public GameObject[] buffs;


    private void Start() {
        currentHp = maxHp;
        healthBar.setMaxHealth(maxHp);
        currentPurifyHp = maxPurifyHp;
    }

    // Calculating damage done to enemy
    public void takeDamage(int dmg){
        int newHp = currentHp - dmg;
        if (newHp <= 0){
            EnemyDeath();
            return;
        }
        currentHp = newHp;
        healthBar.setHealth(newHp);
    } 


    // Calculating purifying damage done to the enemy
    public void purifyDamage(int dmg) {
        currentPurifyHp -= dmg;
        if (currentPurifyHp <= 0) {
            purify();
        }
    }

    public void EnemyDeath() {
        MoneyManager.AddMoney(moneyDrop);
        UIManager.UpdateMoney();

        Destroy(gameObject);
    }

    public void purify() {
        // give the player a purified meal
        PurifyManager.GainMeal();
        UIManager.UpdateMealCount();

        MoneyManager.AddMoney(moneyDrop);
        UIManager.UpdateMoney();

        Destroy(gameObject);
    }
}
