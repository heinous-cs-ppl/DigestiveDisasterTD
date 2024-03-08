using JetBrains.Annotations;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int maxHp = 10;
    [SerializeField]
    private Bar healthBar;
    public int maxPurifyHp = 7;
    public Bar purifyBar;
    public int spawnPointIndex = 0;
    public int damage = 5;
    public float speed = 2f;
    public float attackRadius = 0.55f;
    public int moneyDrop = 10;

    public GameObject[] buffs;


    private void Start()
    {
        healthBar.setMaxValue(maxHp);
        healthBar.setValue(maxHp);

        purifyBar.setMaxValue(maxPurifyHp);
        purifyBar.setValue(0);
    }
    public int getHealth()
    {
        return healthBar.getValue();
    }
    public int getPurifyHealth()
    {
        return purifyBar.getValue();
    }

    // Calculating damage done to enemy
    public void takeDamage(int dmg)
    {
        int currentHp = getHealth();
        int newHp = currentHp - dmg;
        if (newHp <= 0)
        {
            EnemyDeath();
            return;
        }
        healthBar.setValue(newHp);
    }


    // Calculating purifying damage done to the enemy
    public void takePurifyDamage(int dmg)
    {
        int currentPurifyHp = getPurifyHealth();
        int newPurifyHp = currentPurifyHp + dmg;
        if (newPurifyHp >= maxPurifyHp)
        {
            purify();
            return;
        }
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
