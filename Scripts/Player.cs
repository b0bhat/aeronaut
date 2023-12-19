using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody))]
public class Player : Airship
{
    
    public BoostBar boostBar;
    #region Singleton

    public static Player instance;
    void Awake() {
        instance = this;
    }
    #endregion

    public GameObject player;

    new void Start()
    {
        base.Start();
        boostBar.SetMaxBoost(maxBoostAmount);
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        boostBar.SetBoost(currentBoostAmount);
    }

    new public void Recoil(Transform dir, float force) {
        base.Recoil(dir, force);
    }

    new public void TakeDamage(float damage) {
        //hull -= damage;
        //Debug.Log("AHHHHHH");
        base.TakeDamage(damage);
        CinemachineShake.Instance.ShakeCamera(damage, .1f);
    }

    #region Input Methods

    public void OnThrust(InputAction.CallbackContext context) {
        setThrust(context.ReadValue<float>());
    }

    public void OnStrafe(InputAction.CallbackContext context) {
        setStrafe(context.ReadValue<float>());
    }
    public void OnYaw(InputAction.CallbackContext context) {
        setYaw(context.ReadValue<float>());
    }
    public void OnBoost(InputAction.CallbackContext context) {
        boosting = context.performed;
    }
    
    #endregion
}