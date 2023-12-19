using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{

    stat_Maps statMap = null;
    [SerializeField] string mapName;
    [SerializeField] GameObject cityPrefab;
    public GameObject[] cityList;


    #region Singleton

    public static MapLoader instance;
    void Awake() {
        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        statMap = gameObject.GetComponent<stat_Maps>();
        statMap.setCityStats(mapName);
        cityList = new GameObject[statMap.statCities.Count];
        for (int i = 0; i < statMap.statCities.Count; i++) {
            stat_Maps.cityStats cityStat = statMap.statCities[i];
            cityList[i] = Instantiate(cityPrefab, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);
            cityList[i].transform.position = new Vector3(cityStat.location.x, 0, cityStat.location.y);
            cityList[i].GetComponent<CityWorld>().setStats(cityStat);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
