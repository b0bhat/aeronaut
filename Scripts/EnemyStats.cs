using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public Enemy Enemy;
    [SerializeField] public baseShip ship = null;
    [SerializeField] CharacterStat thrust;
    [SerializeField] CharacterStat maneuver;
    [SerializeField] CharacterStat boost;
    [SerializeField] CharacterStat hull;
    //[SerializeField] int weaponCount = 4;
    [SerializeField] public GameObject[] weapons;

    void Start() {
        hull.BaseValue = ship.hull;
        thrust.BaseValue = ship.thrust;
        maneuver.BaseValue = ship.maneuver;
        boost.BaseValue = ship.boost;

        Enemy.maxHull = hull.Value;
        Enemy.hull = hull.Value;
    }

    void FixedUpdate() {

        Enemy.thrust = thrust.Value*(1.0f);
        Enemy.yawTorque = maneuver.Value*(1f/10f);
        Enemy.strafeThrust = maneuver.Value*1f;
    }
}
