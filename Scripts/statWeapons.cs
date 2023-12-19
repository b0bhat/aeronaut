using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statWeapons : MonoBehaviour
{

    public float stat_maxCooldown;
    public float stat_tickRate;
    public int stat_burstCount;
    public float stat_burstTime;
    public float stat_rotateSpeed;
    public int stat_weaponType; // 0 = kinetic, 1 = explosive, 2 = teslamagnetic, 3 = airburst
    public int stat_force; 
    public float stat_damage;
    public float stat_range;

    void Start() {
        stat_maxCooldown = 1f;
        stat_tickRate = 0.1f;
        stat_burstCount = 1;
        stat_burstTime = 1f;
        stat_rotateSpeed = 1f;
        stat_weaponType = 0;
        stat_force = 0;
        stat_damage = 0;
        stat_range = 0;
    }
    
    public void setWeaponStats(string name) {
        if (name == "50mm Cannon") {
            stat_maxCooldown = 15f;
            stat_burstCount = 6;
            stat_burstTime = 0.5f;
            stat_rotateSpeed = 1f;
            stat_weaponType = 0;
            stat_force = 100;
            stat_damage = 10;
            stat_range = 30;
        } else if (name == "280mm Cannon") {
            stat_maxCooldown = 25f;
            stat_burstCount = 1;
            stat_burstTime = 0.5f;
            stat_rotateSpeed = 0.5f;
            stat_weaponType = 1;
            stat_force = 800;
            stat_damage = 30;
            stat_range = 40;
        } else if (name == "35EJ Coilgun") {
            stat_maxCooldown = 30f;
            stat_burstCount = 1;
            stat_burstTime = 1f;
            stat_rotateSpeed = 0.2f;
            stat_weaponType = 2;
            stat_force = 500;
            stat_damage = 5;
            stat_range = 40;
        } else if (name == "130mm Airburst") {
            stat_maxCooldown = 1f;
            stat_burstCount = 1;
            stat_burstTime = 1f;
            stat_rotateSpeed = 0.2f;
            stat_weaponType = 2;
            stat_force = 500;
            stat_damage = 5;
            stat_range = 40;
        }
    }
}
