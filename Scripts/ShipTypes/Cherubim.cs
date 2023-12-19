using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherubim : baseShip
{
    // Firing Groups
    static firingGroup frontal = new firingGroup( new int[] {-45, 45}, new int[] {0,1});

    void Awake() {
        groups = new firingGroup[] {frontal};

        // Weapons Positions
        weaponNum = 2;
        weaponPos = new Vector3[weaponNum];
        weaponPos[0] = new Vector3(0f, 0f, 2.5f);
        weaponPos[1] = new Vector3(0f, 0f, 2.5f);
        
        // Stats
        thrust = 8f;
        maneuver = 12f;
        boost = 2f;
        hull = 240f;
    }
}
