using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform firingPoint;

    // attributes (defined in StudentInfo.cs)
    protected float targetingRange;
    private float rotationSpeed;

    // attributes for the bullets (also defined in StudentInfo.cs)
    private float bulletSpeed;
    private int bulletDamage;
    private float bulletDistance;

    private float bps; // Bullets Per Second


    protected Transform target;
    protected float timeUntilFire;

    [SerializeField] private float spriteRotation = 0f;
    public bool rotateBullet;
    [SerializeField] protected Animator anim;

    


    private void Start()
    {
        StudentInfo student = GetComponent<StudentInfo>();

        SetStudentAttributes(student);
        SetBulletAttributes(student);
        timeUntilFire = 1f / bps;
    }

    public void SetStudentAttributes(StudentInfo student)
    {
        targetingRange = student.range;
        rotationSpeed = student.rotationSpeed;
        bps = student.bps;
    }

    public void SetBulletAttributes(StudentInfo student)
    {
        bulletSpeed = student.bulletSpeed;
        bulletDamage = student.damage;
        bulletDistance = student.bulletDistance;
    }

    // Updates the turret gun to aim at food
    protected void Update()
    {

        // Tries to find food to attack
        if (target == null)
        {
            FindTarget();
            return;
        }

        if (target != null) RotateTowardsTarget();
        

        // Steadily brings gun towards next target instead of snapping 
        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {

            // If an enemy is in range, fire at set rate
            timeUntilFire += Time.deltaTime;

            // If the time until fire is larger then 1/bps 
            if (timeUntilFire >= 1f / bps)
            {
                Shoot();

                // Resets time to fire next shot
                timeUntilFire = 0f;
            }
        }
    }

    // function to actually fire a projectile
    private void Shoot()
    {
        // shoot a projectile with the same rotation as the student (adjusted by the sprite angle)
        GameObject bulletObj;
        if(rotateBullet) {
            Quaternion adjustedRotation = Quaternion.Euler(0, 0, turretRotationPoint.eulerAngles.z - spriteRotation);
            bulletObj = Instantiate(bulletPrefab, firingPoint.position, adjustedRotation);
        } else {
            bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        }
        Bullets bulletScript = bulletObj.GetComponent<Bullets>();
        bulletScript.SetTarget(target);
        bulletScript.SetAttributes(bulletSpeed, bulletDamage, bulletDistance, gameObject.transform.position);

        // set throwing animation
        if(anim != null) anim.SetTrigger("OnThrow");
    }

    // Finds the closest target available
    protected void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        //
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    // Checks if target is in range
    protected bool CheckTargetIsInRange()
    {
        // Returns the 2D vector to check if the target is in range 
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    protected void RotateTowardsTarget()
    {
        // Calculates the enemy that is in range and will rotate turret 
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        // Actually rotates the turret
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        // turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        turretRotationPoint.rotation = targetRotation;


        // Update the sprite based on the direction to the target
        Vector2 facingDirection = turretRotationPoint.transform.up;
        if(anim != null){
            if (Mathf.Abs(facingDirection.x) > Mathf.Abs(facingDirection.y)) {
                if(facingDirection.x < 0) {
                    anim.SetTrigger("FaceLeft");
                } else {
                    anim.SetTrigger("FaceRight");
                }
            } else {
                if(facingDirection.y > 0) {
                    anim.SetTrigger("FaceUp");
                } else {
                    anim.SetTrigger("FaceDown");
                }
            }
        }
        
    }

    // Creates a ring around turret that represents the range in editor (Cannot see in actual game)
    private void OnDrawGizmosSelected()
    {

        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);

    }
}