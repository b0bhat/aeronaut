using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isoroku : baseShip
{
    // Firing Groups
    static firingGroup frontal = new firingGroup( new int[] {-120, 120}, new int[] {0,1,2});

    void Awake() {
        groups = new firingGroup[] {frontal};

        // Weapons Positions
        weaponNum = 3;
        weaponPos = new Vector3[weaponNum];
        weaponPos[0] = new Vector3(0f, 0f, 2.5f);
        weaponPos[1] = new Vector3(0f, 0f, 2.5f);
        weaponPos[2] = new Vector3(0f, 0f, 2.5f);
        
        // Stats
        thrust = 5f;
        maneuver = 7f;
        boost = 2f;
        hull = 300f;
    }
}
