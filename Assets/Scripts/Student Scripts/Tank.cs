using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Turret
{
    private new void Update()
    {

        // Tries to find food to attack
        if (target == null)
        {
            FindTarget();
            return;
        }

        // Rotates gun towards target
        RotateTowardsTarget();


        // Steadily brings gun towards next target instead of snapping 
        if (!CheckTargetIsInRange())
        {
            target = null;
        }
    }
}
