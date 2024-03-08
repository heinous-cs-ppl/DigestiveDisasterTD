using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PurifyBullet : Bullets
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Take health from Enemy
        other.gameObject.GetComponent<EnemyInfo>().takePurifyDamage(bulletDamage);
        Destroy(gameObject);
    }
}
