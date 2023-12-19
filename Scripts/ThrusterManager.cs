using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterManager : MonoBehaviour
{
    private ParticleSystem ps;
    private float currentThrust;

    // Start is called before the first frame update
    void Start()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var speed = ps.main;
        //var vel = ps.velocityOverLifetime;
        speed.startSpeed = Player.instance.velocity/2;
    }
}
