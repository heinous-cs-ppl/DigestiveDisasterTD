using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static TextMeshProUGUI purifyCount;

    void Start() {
        purifyCount = GameObject.Find("Purify Count").GetComponent<TextMeshProUGUI>();
        UIManager.UpdateMealCount();
    }

    public static void UpdateMealCount() {
        purifyCount.text = PurifyManager.GetStringMealCount();
    }
}
