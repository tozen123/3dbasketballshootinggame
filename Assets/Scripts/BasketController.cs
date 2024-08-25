using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasketController : MonoBehaviour
{
    public CameraController cameraController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            cameraController.start = true;
            PlayerPointingSystem.Instance.AddPoint(1);
        }
    }
}
