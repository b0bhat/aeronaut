using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordfish : baseShip
{
    // Firing Groups
    static firingGroup rightSide = new firingGroup( new int[] {45, 135}, new int[] {0,1});
    //static firingGroup leftSide = new firingGroup( new int[] {45, 135}, new int[] {2,3});
    static firingGroup leftSide = new firingGroup(new int[] {-135, -45}, new int[] {2,3});

    void Awake() {
        groups = new firingGroup[] {rightSide, leftSide};

        // Weapons Positions
        weaponNum = 4;
        weaponPos = new Vector3[weaponNum];
        weaponPos[0] = new Vector3(1.8f, 0f, 1f);
        weaponPos[1] = new Vector3(1.8f, 0f, -1f);
        //weaponPos[2] = new Vector3(1.5f, 0f, 1f);
        //weaponPos[3] = new Vector3(1.5f, 0f, -1f);
        weaponPos[2] = new Vector3(-1.8f, 0f, 1f);
        weaponPos[3] = new Vector3(-1.8f, 0f, -1f);
        
        // Stats
        thrust = 8f;
        maneuver = 10f;
        boost = 2f;
        hull = 480f;
    }
}
