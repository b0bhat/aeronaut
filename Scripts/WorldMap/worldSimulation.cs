using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldSimulation : MonoBehaviour
{

    public GameObject[] cityList;
    public GameObject fleetPrefab;
    public List<GameObject> fleetList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        cityList = MapLoader.instance.cityList;
        //Instantiate(fleetPrefab, new Vector3(29,0,30), Quaternion.identity);
        foreach (GameObject city in cityList) {
            int import = city.GetComponent<CityWorld>().cityStat.importance;
            for (int i = 0; i < import; i++) {
                var rad = (2*Mathf.PI/import * i);
                var spawnPos = city.transform.position + new Vector3(Mathf.Cos(rad),0,Mathf.Sin(rad)) * 30;
                GameObject fleet = Instantiate(fleetPrefab, spawnPos, Quaternion.identity);
                fleet.GetComponent<FleetWorld>().setGuard(city.transform.position);
                fleet.GetComponent<FleetWorld>().flightType = FleetWorld.FlightType.Patrol;
                fleet.GetComponent<FleetWorld>().setShips(
                    city.GetComponent<CityWorld>().cityStat.owner,
                    city.GetComponent<CityWorld>().cityStat.importance
                );
                //fleet.GetComponent<FleetWorld>().setFollow(AirshipWorld.instance.transform.position);
                fleetList.Add(fleet);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
