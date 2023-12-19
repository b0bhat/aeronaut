using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.VFX;

[RequireComponent (typeof(Rigidbody))]
public class Enemy : Airship
{
    private enum State {
        Aggressive,
        HitAndRun,
        Formation,
        Cautious,
        Retreat,
        Run,
        Dead,
    }

    [SerializeField] GameObject remains;

    public GameObject Explode;
    public float lookRadius = 50f;
    public float runRadius = 20f;
    public float strikeRadius = 24f;
    public bool running = false;
    Transform target;
    NavMeshAgent agent;
    [SerializeField] EnemyStats stats;

    private State state;
    public Vector3 dir;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        target = Player.instance.transform;
        agent = GetComponent<NavMeshAgent>();
        state = State.HitAndRun;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        if (state !=  State.Dead) {
            switch (state) {
                default:
                case State.HitAndRun:
                    HitAndRun();
                    break;
                case State.Cautious:
                    Cautious();

                    break;
            }
        CheckHealth();
        }
        //rb.AddRelativeForce(Vector3.forward * 10f * 10f * Time.fixedDeltaTime);
    }

    void HitAndRun() {
        float distance = Vector3.Distance(target.position, transform.position);
        dir = (target.position - transform.position).normalized;
        dir.y = 0f;
        if (distance <= runRadius && running == false) running = true;
        if (distance <= lookRadius && distance > strikeRadius && running == true) running = false;
        if (running) {
            transform.forward = Vector3.RotateTowards(
                transform.forward, dir, -yawTorque * Time.deltaTime, 0.0f
            ); transform.Translate(Vector3.forward * thrust * Time.deltaTime);
        } else if (!running) {
            transform.forward = Vector3.RotateTowards(
                transform.forward, dir, yawTorque * Time.deltaTime, 0.0f
            ); transform.Translate(Vector3.forward * thrust * Time.deltaTime);
            //agent.SetDestination(target.position);
        }

    }

    void Cautious() {
        float distance = Vector3.Distance(target.position, transform.position);
        dir = (target.position - transform.position).normalized;
        if (distance <= strikeRadius && running == false) running = true;
        if (distance <= lookRadius && distance > strikeRadius+5 && running == true) running = false;
        if (running) {
            transform.forward = Vector3.RotateTowards(
                transform.forward, dir, -yawTorque * Time.deltaTime, 0.0f
            ); transform.Translate(Vector3.forward * thrust * Time.deltaTime);
        } else if (!running) {
            if (distance > strikeRadius + 5) {
                transform.forward = Vector3.RotateTowards(
                    transform.forward, dir, yawTorque * Time.deltaTime, 0.0f
                );
                transform.Translate(Vector3.forward * thrust * Time.deltaTime);
            }
            //agent.SetDestination(target.position);
        }
        //Debug.Log(target.position);
    }

    private void CheckHealth() {
        if (hull <= 0) {
            //Instantiate(remains, transform.position, transform.rotation);
            Instantiate(Explode, transform.position, transform.rotation);
            state = State.Dead;
            gameObject.GetComponent<WeaponsEnemy>().setDead();
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
