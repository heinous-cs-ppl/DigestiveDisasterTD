using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int maxHp = 10;
    [HideInInspector] public int currentHp;
    public Bar healthBar;
    public int maxPurifyHp = 7;
    [HideInInspector] public int currentPurifyHp;
    public Bar purifyBar;
    public int spawnPointIndex = 0;
    public int damage = 5;
    public float speed = 2f;
    public float attackRadius = 0.55f;
    public int moneyDrop = 10;

    public GameObject[] buffs;


    private void Start()
    {
        currentHp = maxHp;
        healthBar.setMaxValue(maxHp);
        healthBar.setValue(maxHp);

        currentPurifyHp = 0;
        purifyBar.setMaxValue(maxPurifyHp);
        purifyBar.setValue(0);
    }

    // Calculating damage done to enemy
    public void takeDamage(int dmg)
    {
        int newHp = currentHp - dmg;
        if (newHp <= 0)
        {
            EnemyDeath();
            return;
        }
        currentHp = newHp;
        healthBar.setValue(newHp);
    }


    // Calculating purifying damage done to the enemy
    public void purifyDamage(int dmg)
    {
        int newPurifyHp = currentPurifyHp + dmg;
        if (newPurifyHp >= maxPurifyHp)
        {
            purify();
            return;
        }
        currentPurifyHp = newPurifyHp;
        purifyBar.setValue(newPurifyHp);
    }

    public void EnemyDeath()
    {
        MoneyManager.AddMoney(moneyDrop);
        UIManager.UpdateMoney();

        Destroy(gameObject);
    }

    public void purify()
    {
        // give the player a purified meal
        PurifyManager.GainMeal();
        UIManager.UpdateMealCount();

        MoneyManager.AddMoney(moneyDrop);
        UIManager.UpdateMoney();

        Destroy(gameObject);
    }
}
