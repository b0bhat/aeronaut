using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityWorld : MonoBehaviour
{
    float hover = 0.5f;
    public stat_Maps.cityStats cityStat;
    // Start is called before the first frame update
    void Start()
    {
        CheckDown(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckDown(Transform check) {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(check.position, Vector3.down, out hit)) {
            if (hit.distance != hover) {
                float diff = hit.distance - hover;
                check.position = new Vector3(check.position.x, check.position.y -diff, check.position.z);
            }
        }
    }

    public void setStats(stat_Maps.cityStats stat) {
        cityStat = stat;
        //Debug.Log(cityStat.name);
    }
}
