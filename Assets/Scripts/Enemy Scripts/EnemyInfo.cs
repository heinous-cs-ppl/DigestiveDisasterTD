using System.Collections;
using JetBrains.Annotations;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int maxHp = 10;

    [SerializeField]
    public Bar healthBar;
    public int maxPurifyHp = 7;
    public Bar purifyBar;
    public int spawnPointIndex = 0;
    public int damage = 5;
    private int originalDamage;
    public float speed = 2f;
    public float attackRadius = 0.55f;
    public int moneyDrop = 10;

    SpriteRenderer enemySprite;

    public GameObject[] buffs;

    public bool isBoss = false;

    [HideInInspector]
    public bool LawyerDebuff = false;

    [HideInInspector]
    public float LawyerDamageMultiplier;

    private void Start()
    {
        healthBar.setMaxValue(maxHp);
        healthBar.setValue(maxHp);

        purifyBar.setMaxValue(maxPurifyHp);
        purifyBar.setValue(0);

        originalDamage = damage;
        enemySprite = gameObject.GetComponent<SpriteRenderer>();
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
        Debug.Log("Damaged");

        // If the lawyer debuff is true, multiply it by the value that was randomly selected
        if (LawyerDebuff == true)
        {
            dmg = (int)(dmg * LawyerDamageMultiplier);
        }
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
        if (LawyerDebuff == true)
        {
            dmg = (int)(dmg * LawyerDamageMultiplier);
        }
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

        // if the enemy is a boss, set bossAlive to false in spawner
        if (isBoss)
        {
            Spawner.instance.bossAlive = false;
            Debug.Log("Boss died");
        }
        else if (Spawner.instance.isBossWave)
        {
            // if current wave is a boss wave, reduce the number of alive enemies of corresponding tag by 1
            Spawner.instance.ReduceBossEnemyCount(gameObject.tag);
            Debug.Log(gameObject.tag + " killed");
        }

        Destroy(gameObject);
    }

    public void purify()
    {
        // give the player a purified meal
        PurifyManager.GainMeal();
        UIManager.UpdateMealCount();

        MoneyManager.AddMoney(moneyDrop);
        UIManager.UpdateMoney();

        // if the enemy is a boss, set bossAlive to false in spawner
        if (isBoss)
        {
            Spawner.instance.bossAlive = false;
            Spawner.instance.anySpawning = false;
            Debug.Log("Boss died");
        }
        else if (Spawner.instance.isBossWave)
        {
            // if current wave is a boss wave, and this enemy isn't the boss, reduce the number of alive enemies of corresponding tag by 1
            Spawner.instance.ReduceBossEnemyCount(gameObject.tag);
            Debug.Log(gameObject.tag + " purified");
        }

        Destroy(gameObject);
    }

    public void ResetDebuff()
    {
        damage = originalDamage;
        LawyerDebuff = false;
    }

    public void Debuff(float time)
    {
        LawyerDamageMultiplier = 1.3f;
        gameObject.GetComponent<EnemyAttacks>().DamageReduction(1.3f, time);

        enemySprite.color = Color.red;
        Debug.Log("changed enemy color");
        LawyerDebuff = true;
        StartCoroutine(LawyerDelay(time));
    }

    public IEnumerator LawyerDelay(float time)
    {
        yield return new WaitForSeconds(time);
        ResetDebuff();
        enemySprite.color = Color.white;
    }
}
