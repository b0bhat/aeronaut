using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Shield : MonoBehaviour
{
    private Transform aimTransform;
    [SerializeField] GameObject Airship;
    public Vector3 worldPos;
    public Vector3 mousePos;
    public float angle;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Transform airshipT = Airship.transform;
        //// MOUSE RETICLE////
        //mousePos = Mouse.current.position.ReadValue();
        //Debug.Log(mousePos);
        Plane plane = new Plane(Vector3.up, 0);
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        angle = 0;
        Vector3 aimDirection = new Vector3(0,0,0);

        if (plane.Raycast(ray, out distance)) {
            worldPos = ray.GetPoint(distance);
            //Debug.Log(distance);
            //aimTransform.transform.position = worldPosition;
            //Vector3 aimDirection = (worldPos - aimTransform.position).normalized;
            aimDirection = (worldPos - transform.position).normalized;
            Debug.DrawLine(transform.position, worldPos, Color.red, Mathf.Infinity);
            //Debug.Log(aimDirection);
            angle = Mathf.Atan2(aimDirection.x, aimDirection.z) * Mathf.Rad2Deg;
            //Debug.Log(angle);
            transform.eulerAngles = new Vector3(0,angle,0);
        }
    }

    #region Input Methods

    public void Aim(InputAction.CallbackContext context) {
        mousePos = context.ReadValue<Vector2>();
    }
    #endregion
}
