using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Lawyer : MonoBehaviour
{
    private PathFollow PathFollowScript;
    private EnemyInfo EnemyInfoScript;
    private int duration;
    GameObject[] enemies;
    private int debuff;
    private int attackReductionStrength;
    private float slowStrength; 
     int IncreasedDamageMultiplier;

    // NOTE THAT THE RANGE FUNCTION DOES NO INCLUDE THE MAX VALUE, DO NO TRUST IN GOOGLE
    // I HAVE TESTED THIS MULTIPLE TIMES AND IT NEVER INCLUDES THE MAX! 

    void Start()
    {
        PathFollowScript = GetComponent<PathFollow>();
        EnemyInfoScript = GetComponent<EnemyInfo>();
    }
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debuffs(enemies);
    }
    void Awake()
    {
        // Chooses which debuff to use from 1 to 3 
        debuff = Random.Range(1,4);
        Debug.Log(debuff);
        if (debuff == 1)
        {
            duration = Random.Range(3, 7);
            // Strength of the slow
            slowStrength = Random.Range(0.2f, 0.9f);
        } 
        else if (debuff == 2)
        {
            // Strength of damage reduced
            duration = Random.Range(5, 9);
            attackReductionStrength = Random.Range(2, 4);

        } 
        else 
        {
            duration = Random.Range(3,7);
            // Will increase the damage multiplier of the buff before returning it back to it's original 
            IncreasedDamageMultiplier = Random.Range(2,4);
        }
    }

    void Debuffs(GameObject[] enemies)
    {
        if (debuff == 1)
        {
            // slowing debuff

            Debug.Log("Slowing");
        
            // Changes speed of all enemies to the desired speed
            foreach (GameObject obj in enemies)
            {
                obj.gameObject.GetComponent<PathFollow>().StatReset();
                obj.gameObject.GetComponent<PathFollow>().slowSpeed(slowStrength, duration);
                obj.gameObject.GetComponent<PathFollow>().LawyerDebuff = true;
            }
            
        } 
        else if(debuff == 2)
        {
            // damage reduction debuff
    
            Debug.Log("Enemies have their damage reduced");
            foreach (GameObject obj in enemies)
            {
                obj.gameObject.GetComponent<EnemyAttacks>().ResetDebuff();
                obj.gameObject.GetComponent<EnemyAttacks>().DamageReduction(attackReductionStrength, duration);
            }
        } 
        else
        {
           
            foreach (GameObject obj in enemies)
            {
                Debug.Log("Increased Damage");
                obj.gameObject.GetComponent<EnemyInfo>().ResetDebuff();
                obj.gameObject.GetComponent<EnemyInfo>().LawyerDamageMultiplier = IncreasedDamageMultiplier;
                obj.gameObject.GetComponent<EnemyInfo>().LawyerDebuff = true;
                StartCoroutine(NormalDamageMultiplier(duration, enemies));

            }

        }

        // destroys game object after the duration
        Destroy(gameObject, duration);
    }
    IEnumerator NormalDamageMultiplier(int remainingTime, GameObject[] enemies) 
    {
        // Will delay before reseting the attack back to its original speed
        yield return new WaitForSeconds(remainingTime); 
        foreach (GameObject obj in enemies)
        {
            obj.gameObject.GetComponent<EnemyInfo>().LawyerDebuff = false;
            Debug.Log("Back to original damage");   
        }
        
    }
}
