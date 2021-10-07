using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    CinemachineVirtualCamera vcam;

    float originalOrthographicSize = 10f;

    private void Start()
    {
        vcam = this.GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        vcam.Priority = 3;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        vcam.Priority = 1;
        
    }
}
