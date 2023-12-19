using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Airship : MonoBehaviour
{
    private enum Hull {
         full,
         one,
         two,
         dead,
     }


    [Header("=== Ship Movement ===")]
    [SerializeField] public float thrust = 10f;
    [SerializeField] public float yawTorque = 30f;
    [SerializeField] public float strafeThrust = 5f;
    [SerializeField] public float hull = 5f;

    [SerializeField, Range(0.01f, 4.99f)] private float thrustGlideReduction = 0.01f;
    [SerializeField, Range(0.01f, 4.99f)] private float strafeGlideReduction = 0.01f;
    [SerializeField, Range(0.01f, 4.99f)] private float yawGlideReduction = 0.01f;

    [Header("=== Ship Boost ===")]
    [SerializeField] public float maxBoostAmount = 2f;
    [SerializeField] private float boostDeprecationRate = 0.25f;
    [SerializeField] private float boostRechargeRate = 0.05f;
    [SerializeField] public float boostMultiplier = 5f;

    // Public
    public bool boosting = false;
    public bool strafing = false;
    public float currentBoostAmount;
    public float velocity;

    float glide, horizontalGlide, angularGlide = 0f;
    Rigidbody rb;
    private float thrust1D;
    public float strafe1D;
    private float yaw1D;

    private Hull hullState;
    public float maxHull = 5f;

    [SerializeField] GameObject smokeDamage;
    [SerializeField] GameObject glowRing;
    GameObject line;
    public Material mat;

    // Start is called before the first frame update
    public void Start()
    {
        hull = maxHull;
        rb = GetComponent<Rigidbody>();
        currentBoostAmount = maxBoostAmount;
        velocity = 0f;

        //makeShape.instance.MakeCircle(larc, 6, 30, 0);
    }
    // Update is called once per frame
    public void FixedUpdate()
    {
        HandleMovement();
        HandleBoosting();
    }

    public void setThrust(float input) {
        thrust1D = input;
    }

    public void setStrafe(float input) {
        strafe1D = input;
    }

    public void setYaw(float input) {
        yaw1D = input;
    }

    void HandleBoosting() {
        if (boosting && currentBoostAmount > 0f) {
            currentBoostAmount -= boostDeprecationRate;
            if (currentBoostAmount <= 0f) {
                boosting = false;
                strafing = false;
            }
        } else {
            if (currentBoostAmount < maxBoostAmount) {
                currentBoostAmount += boostRechargeRate;
            }
        }
    }

    public void Recoil(Transform dir, float force) {
        rb.AddForce(dir.forward * -force);
    }

    void HandleMovement() {
        velocity = transform.InverseTransformDirection(rb.velocity).z;
        // Yaw
        //rb.AddRelativeTorque(Vector3.up * yaw1D * yawTorque * Time.deltaTime);
        // Yaw
        if (yaw1D > 0.1f || yaw1D < -0.1f) {
            rb.AddRelativeTorque(Vector3.up * yaw1D * yawTorque * Time.fixedDeltaTime);
            if (boosting) {
                angularGlide = yaw1D * yawTorque * boostMultiplier;
            } else {
                angularGlide = yaw1D * yawTorque;
            }
        }
        else {
            rb.AddRelativeTorque(Vector3.up * angularGlide * Time.fixedDeltaTime);
            angularGlide *=  yawGlideReduction;
        }

        // Thrust
        if (thrust1D > 0.5f || thrust1D < -0.5f) {
            float currentThrust = thrust;
            if (boosting) {
                currentThrust = thrust * boostMultiplier;
            } else {
                currentThrust = thrust;
            }
            rb.AddRelativeForce(Vector3.forward * thrust1D * currentThrust * Time.deltaTime);
            glide = thrust;
        }
        else {
            rb.AddRelativeForce(Vector3.forward * glide * Time.deltaTime);
            glide *=  thrustGlideReduction;
        }

        // Strafe
        if (strafe1D > 0.99f || strafe1D < -0.99f) {
            rb.AddRelativeForce(Vector3.right * strafe1D * strafeThrust * Time.fixedDeltaTime);
            if (currentBoostAmount > 20 * boostDeprecationRate) {
                horizontalGlide = strafe1D * strafeThrust * 10 * boostMultiplier;
                currentBoostAmount -= 25 * boostDeprecationRate;
                strafing = false;
            }
        }
        else {
            rb.AddRelativeForce(Vector3.right * horizontalGlide * Time.fixedDeltaTime);
            horizontalGlide *= strafeGlideReduction;
        }
    }

    public void TakeDamage(float damage) {
        hull -= damage;
        if ((hull < (2*maxHull)/3) && (hullState == Hull.full)) {
            Instantiate(smokeDamage,
                new Vector3(transform.position.x+ Random.Range(-0.5f,0.5f), transform.position.y,transform.position.z+Random.Range(-1f,1f)),
                Quaternion.identity, gameObject.transform);
            hullState = Hull.one;
        } else if ((hull < (maxHull)/3) && (hullState == Hull.one)) {
            hullState = Hull.two;
            Instantiate(smokeDamage,
                new Vector3(transform.position.x+ Random.Range(-0.5f,0.5f), transform.position.y,transform.position.z+Random.Range(-1f,1f)),
                Quaternion.identity, gameObject.transform);
        } else if ((hull <= 0) && (hullState == Hull.two)) {
            hullState = Hull.dead;
            Instantiate(smokeDamage,
                new Vector3(transform.position.x+ Random.Range(-0.5f,0.5f), transform.position.y,transform.position.z+Random.Range(-1f,1f)),
                Quaternion.identity, gameObject.transform);
            Instantiate(smokeDamage,
                new Vector3(transform.position.x+ Random.Range(-0.5f,0.5f), transform.position.y,transform.position.z+Random.Range(-1f,1f)),
                Quaternion.identity, gameObject.transform);
            OverlayView.instance.DeadCircle(gameObject);
        }
    }
}
