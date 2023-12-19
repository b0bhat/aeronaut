using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //private Vector3 startPos;
    public float lookRadius = 10f;

    private void Start() {
        //startPos = transform.position;
    }
/*
    private Vector3 GetRoamPos() {
        var randomdir = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        return startPos + randomdir * Random.Range(10f, 70f);
    }*/

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

}
