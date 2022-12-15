using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI healthDisplay;
    public void SetMaxHealth(int health)
    {
        //float orgMaxHealth = slider.maxValue;
        slider.maxValue = health;
        slider.value = health;//回復至最大血量

        fill.color = gradient.Evaluate(1f);
        healthDisplay.text = slider.value.ToString() + " / " + slider.maxValue.ToString();
    }
    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        healthDisplay.text = slider.value.ToString() + " / " + slider.maxValue.ToString();
    }
}
