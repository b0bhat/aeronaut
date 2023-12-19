using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject target;
    [SerializeField] float yaw = 1f;
    [SerializeField] float thrust = 30f;
    [SerializeField] float maxSpeed = 30f;
    [SerializeField] float fuel = 100f;
    [SerializeField] float damage = 300;
    [SerializeField] bool tracking = false;

    int destroyTick = 1000;
    bool dead = false;
    int timeAlive = 0;

    [SerializeField] GameObject explosion;
    [SerializeField] GameObject missile;
    [SerializeField] GameObject sign;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive++;
        if (fuel > 0 && !dead) {
            var dir = (target.transform.position - transform.position);
            /*transform.forward = Vector3.RotateTowards(
                transform.forward, dir, yaw * Time.deltaTime, 0.0f
            ); //transform.Translate(Vector3.forward * thrust * Time.deltaTime);*/

            var rotation = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, yaw * Time.deltaTime));
            //yaw = yaw - yaw/300;
            rb.velocity = transform.forward * thrust;
            //rb.AddRelativeForce(transform.forward*thrust);
            if (thrust < maxSpeed) thrust += 0.3f;
            fuel--;
        } if (timeAlive >= 1000 || dead) Dead();
        //OverlayView.instance.RotateMissile(sign); //RE-ENABLE FOR TRIANGLE


    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.transform.TryGetComponent(out Player player)) {
            Boom();
            player.TakeDamage(damage);
            Dead();
        } else if(collision.transform.TryGetComponent(out Airship enemy)) {
            Boom();
            enemy.TakeDamage(damage);
            Dead();
        }
    }

    private void Dead() {
        if (sign) DestroySign();
        missile.SetActive(false);
        rb.velocity = new Vector3(0,0,0);
        gameObject.GetComponent<Collider>().enabled = false;
        dead = true;
        destroyTick--;
        if (destroyTick <= 0) Destroy(gameObject);
    }

    public void setSign(GameObject triangle) {
        sign = triangle;
    }

    private void Boom() {
        Instantiate(explosion, transform.position, Quaternion.identity);
        if (sign) DestroySign();
    }

    private void DestroySign() {
        OverlayView.instance.missileSignDelete(sign);
        GameObject.Destroy(sign);
    }

    public void SetTarget(GameObject enemy) {
        target = enemy;
    }
}
