using UnityEngine;
using System.Collections;

public class LawyerStudent : Turret
{
    
    private new void Update() {
        timeUntilFire += Time.deltaTime;
        if (timeUntilFire >= 1f / bps) {
            
            GameObject[] targets = FindTargets();
            if (targets.Length > 0) {
                anim.SetTrigger("OnThrow");
                StartCoroutine(EndThrow());
                Debug.Log("Found targets");
                Debuff(targets);
                timeUntilFire = 0;
            }

            
        }
    }

    private GameObject[] FindTargets() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);
        GameObject[] targets = new GameObject[hits.Length];
        for (int i = 0; i < hits.Length; i++) {
            Debug.Log("target" + i);
            targets[i] = hits[i].gameObject;
        }
        return targets;
    }

    private void Debuff(GameObject[] targets) {
        foreach (GameObject target in targets) {
            EnemyInfo enemyInfo = target.GetComponent<EnemyInfo>();
            enemyInfo.Debuff((1f/bps) * 0.8f);
            Debug.Log("Debuffed " + target.name);
        }
    }

    
}