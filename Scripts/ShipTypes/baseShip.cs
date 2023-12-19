using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseShip : MonoBehaviour
{
    public struct firingGroup {
        public int[] angles {get; set;}
        public int[] weaponsInGroup {get; set;}
        public firingGroup(int[] angles, int[] weaponsInGroup) {
            this.angles = angles;
            this.weaponsInGroup = weaponsInGroup;
        }
    }
    public float thrust = 1f;
    public float maneuver = 1f;
    public float boost = 1f;
    public float hull = 1f;
    public firingGroup[] groups;
    public Vector3[] weaponPos;
    public int weaponNum;
}
