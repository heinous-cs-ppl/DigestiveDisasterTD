using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStudent : MonoBehaviour
{
    public void Fire() {
        GameObject selected = StudentManager.selected;
        
        // Vacuous students don't have a turret
        if (selected.GetComponent<StudentInfo>().turret != null) {
            // refund a fraction of the student's cost
            MoneyManager.AddMoney((int) (0.5 * selected.GetComponent<StudentInfo>().cost));
            UIManager.UpdateMoney();

            Destroy(selected);
            StudentManager.plotOfSelected.GetComponent<Plot>().student = null;
            StudentManager.Deselect();
        }
    }
}
