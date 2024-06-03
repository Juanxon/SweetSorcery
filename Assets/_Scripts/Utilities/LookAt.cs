using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Camera myCamera;

    private void OnEnable()
    {
        myCamera = Camera.main;
    }
    private void FixedUpdate()
    {
        transform.rotation = myCamera.transform.rotation;
    }
}
