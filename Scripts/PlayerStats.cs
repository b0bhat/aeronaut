using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public Player Player;
    [SerializeField] public baseShip ship = null;
    [SerializeField] CharacterStat thrust;
    [SerializeField] CharacterStat maneuver;
    [SerializeField] CharacterStat boost;
    [SerializeField] CharacterStat hull;
    [SerializeField] public int weaponCount = 4;
    [SerializeField] public GameObject[] weapons;
    [SerializeField] public GameObject missile;
    string sceneName;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        //assigns ship stats on start
        hull.BaseValue = ship.hull;
        thrust.BaseValue = ship.thrust;
        maneuver.BaseValue = ship.maneuver;
        boost.BaseValue = ship.boost;

        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "CombatScene") {
            Player.maxHull = hull.Value;
            Player.hull = hull.Value;
            Player.thrust = thrust.Value*300f;
            Player.yawTorque = maneuver.Value*100f;
            Player.strafeThrust = maneuver.Value*100f;
            Player.boostMultiplier = boost.Value;
        } else if (sceneName == "World Map") {

        }
    }

    void FixedUpdate() {

    }
}
