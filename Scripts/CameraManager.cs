using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    //[SerializeField] private CinemachineVirtualCamera myCineMachine = null;
    [SerializeField] private GameObject followPointer = null;
    [SerializeField] private GameObject unitGameObject = null;
    [SerializeField] private int view = 15;
    Vector3 followPointerPos = new Vector3();

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.nearClipPlane*view;
        
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        //Debug.Log(mousePos);
        followPointerPos = (mousePos + unitGameObject.transform.position) / 2;
        //followPointerPos = (followPointerPos + unitGameObject.transform.position) / 2;
        followPointer.transform.position = followPointerPos;
        //followPointer.transform.position *= 2f;
    }
}
