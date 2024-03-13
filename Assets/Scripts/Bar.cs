using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public void setMaxValue(float value)
    {
        slider.maxValue = value;
    }
    public int getMaxValue()
    {
        return (int)slider.maxValue;
    }
    public void setValue(float value)
    {
        slider.value = value;
    }
    public int getValue()
    {
        return (int)slider.value;
    }
}
