using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    [SerializeField] GameObject missile = null;
    public float cooldown = 0;
    float tickrate = 1;
    [SerializeField] float maxCooldown = 50;
    [SerializeField] int swarm = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Check(bool launch) {
        if (cooldown == 0 && launch) {
            gameObject.GetComponent<ParticleSystem>().Play();
            Vector3 launchPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+5, gameObject.transform.position.z-2);
            for (int i = 0; i < swarm; i++) {
                GameObject missileLaunched = Instantiate(missile, launchPos, gameObject.transform.localRotation*Quaternion.Euler(-90,0,0), GameObject.Find("MissileHolder").transform);
                missileLaunched.GetComponent<Missile>().SetTarget(GameObject.Find("Enemy_Cherubim")); // TODO placeholder enemy target search
                //GameObject triangle = OverlayView.instance.missileSign(missileLaunched); //RE-ENABLE FOR TRIANGLE
                //missileLaunched.GetComponent<Missile>().setSign(triangle); //RE-ENABLE FOR TRIANGLE
            }
            cooldown = maxCooldown;
        } else {
            cooldown -= tickrate;
            if (cooldown < 0) cooldown = 0;
        }
    }

    public void SetMissile(GameObject set) {
        missile = set;
        //Debug.Log(set);
    }
}
