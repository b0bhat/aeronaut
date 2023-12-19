using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject rotate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Vector3.RotateTowards(
                transform.forward, (rotate.transform.position - transform.position).normalized, 0.01f * Time.deltaTime, 0.0f
            );
        transform.Translate(Vector3.forward * 1 * Time.deltaTime);
    }
}
