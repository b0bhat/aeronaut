using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class stat_Nations : MonoBehaviour
{
    public struct nationShip {
        public GameObject name {get; set;}
        public float chance {get; set;}
        public string shipClass {get; set;}
        public nationShip(GameObject name, float chance, string shipClass) {
            this.name = name;
            this.chance = chance;
            this.shipClass = shipClass;
        }
    }

    void Start() {

    }

    public List<nationShip> getShips(string owner) {
        List<nationShip> statNationShips = new List<nationShip>();
        try {
            var load = Resources.LoadAll(owner + "/", typeof(GameObject)).Cast<GameObject>();
            foreach(var go in load) {
                if (go.name == "cherubimgame") statNationShips.Add(new nationShip(go, 1f, "frigate"));
            }
        } catch {
            Debug.Log("loading fail");
        }

        return statNationShips;
    }
}
