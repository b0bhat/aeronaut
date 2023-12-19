using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stat_Maps : MonoBehaviour
{
    public struct cityStats {
        public Vector2 location {get; set;}
        public string name {get; set;}
        public string owner {get; set;}
        public int population {get; set;}
        public int importance {get; set;}
        public cityStats(Vector2 location, string name, string owner, int population, int importance) {
            this.location = location;
            this.name = name;
            this.owner = owner;
            this.population = population;
            this.importance = importance;
        }
    }
    public List<cityStats> statCities = new List<cityStats>();

    // Start is called before the first frame update
    void Start()
    {
        statCities.Add(new cityStats(new Vector2(0,0), "defaultCity", "Independant", 1, 1));
    }

    public void setCityStats(string name) {
        if (name == "Conclave1") {
            statCities.Add(new cityStats(new Vector2(-196,-8), "Conclave1_city0", "Conclave", 2, 1));
            statCities.Add(new cityStats(new Vector2(2,-291), "Conclave1_city1", "Conclave", 1, 2));
            statCities.Add(new cityStats(new Vector2(-207,-163), "Conclave1_city2", "Conclave", 3, 3));
            statCities.Add(new cityStats(new Vector2(46,100), "Conclave1_city3", "Conclave", 3, 4));
            statCities.Add(new cityStats(new Vector2(-6,294), "Conclave1_city4", "Conclave", 1, 2));
            statCities.Add(new cityStats(new Vector2(213,-117), "Conclave1_city5", "Conclave", 1, 2));
        }
    }
}
