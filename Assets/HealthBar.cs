using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health) // This function sets the maximum health of the player
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health) // This function sets the current health of the player
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}

