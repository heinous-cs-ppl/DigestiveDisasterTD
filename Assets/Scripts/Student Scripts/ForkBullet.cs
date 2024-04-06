using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkStudent : Bullets
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Take health from Enemy
        other.gameObject.GetComponent<EnemyInfo>().takeDamage(bulletDamage);
        PlaySound.instance.SFX(sfx);
        Destroy(gameObject);
    }
}
