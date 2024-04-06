using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerBullet : Bullets
{

    public GameObject explosion;
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Changes to desired prefab for aoe
        Instantiate(explosion, transform.position, transform.rotation);
        PlaySound.instance.SFX(sfx);
        Destroy(gameObject);
    }
}
