using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class PurifyTurret : Turret
{
    // [Header("References")]
    // [SerializeField] private Transform turretRotationPoint;
    // [SerializeField] private LayerMask enemyMask;
    // [SerializeField] private GameObject bulletPrefab;

    // [SerializeField] private Transform firingPoint;

    // // attributes (defined in StudentInfo.cs)
    // private float targetingRange;
    // private float rotationSpeed;

    // // attributes for the bullets (also defined in StudentInfo.cs)
    // private float bulletSpeed; 
    // private int bulletDamage;
    // private float bulletLifetime;

    // private float bps; // Bullets Per Second


    // private Transform target; 
    // private float timeUntilFire; 


    // private void Start() {
    //     StudentInfo student = GetComponent<StudentInfo>();
    //     targetingRange = student.range;
    //     rotationSpeed = student.rotationSpeed;
    //     bps = student.bps;

    //     bulletSpeed = student.bulletSpeed;
    //     bulletDamage = student.damage;
    //     bulletLifetime = student.bulletLifetime;
    // }

    // Updates the turret gun to aim at food
    // private void Update(){
        
    //     // Tries to find food to attack
    //     if (target == null){
    //         FindTarget();
    //         return;
    //     }

    //     // Rotates gun towards target
    //     RotateTowardsTarget();


    //     // Steadily brings gun towards next target instead of snapping 
    //     if (!CheckTargetIsInRange()){
    //         target = null;
    //     } else {

    //         // If an enemy is in range, fire at set rate
    //         timeUntilFire += Time.deltaTime;

    //         // If the time until fire is larger then 1/bps 
    //         if (timeUntilFire >= 1f / bps){
    //             Shoot();

    //             // Resets time to fire next shot
    //             timeUntilFire = 0f;
    //         } 
    //     }
    // }

    // function to actually fire a projectile
    // private void Shoot(){
    //     GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
    //     PurifyBullet bulletScript = bulletObj.GetComponent<PurifyBullet>();
    //     bulletScript.SetTarget(target); 
    //     bulletScript.SetAttributes(bulletSpeed, bulletDamage, bulletLifetime);
    // }

    // Finds the closest target available
    // private void FindTarget(){
    //     RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2) transform.position, 0f, enemyMask);

    //     //
    //     if (hits.Length > 0){
    //         target = hits[0].transform;
    //     }
    // }

    // Checks if target is in range
    // private bool CheckTargetIsInRange(){
    //     // Returns the 2D vector to check if the target is in range 
    //     return Vector2.Distance(target.position, transform.position) <= targetingRange;
    // }

    // private void RotateTowardsTarget(){
    //     // Calculates the enemy that is in range and will rotate turret 
    //     float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;        

    //     // Actually rotates the turret
    //     Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    //     turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    // }

    // Creates a ring around turret that represents the range in editor (Cannot see in actual game)
    // private void OnDrawGizmosSelected(){

    //     Handles.color = Color.cyan;
    //     Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);

    // }
}