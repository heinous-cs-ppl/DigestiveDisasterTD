using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStudent : MonoBehaviour
{
    public void Fire()
    {
        GameObject selected = StudentManager.selected;

        // Vacuous students don't have a turret
        if (selected.GetComponent<StudentInfo>().turret != null)
        {
            // if the first round hasn't started, refund the full cost
            // otherwise, refund a fraction of the cost
            if (Spawner.instance.GetRound() != -1)
            {
                MoneyManager.AddMoney((int)(0.5 * selected.GetComponent<StudentInfo>().cost));
            }
            else
            {
                MoneyManager.AddMoney((int)(selected.GetComponent<StudentInfo>().cost));
            }

            UIManager.UpdateMoney();

            Destroy(selected);
            Plot plotOfFired = StudentManager.plotOfSelected.GetComponent<Plot>();
            if (selected.GetComponent<StudentInfo>().turret is MachineStudent)
            {
                if (plotOfFired.plotOnLeft)
                {
                    plotOfFired.plotOnLeft.gameObject.GetComponent<Plot>().student = null;
                }
            }
            plotOfFired.student = null;
            StudentManager.Deselect();
        }
    }
}
