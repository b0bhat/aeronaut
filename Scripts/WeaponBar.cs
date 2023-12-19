using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxCooldown(float maxCooldown) {
        slider.maxValue = maxCooldown;
        slider.value = maxCooldown;
    }

    public void SetCooldown(float cooldown) {
        slider.value = cooldown;
    }
}
