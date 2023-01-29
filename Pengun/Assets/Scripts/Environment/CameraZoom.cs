using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineConfiner confiner;
    public PolygonCollider2D roomCollider;
    public float zoomInSize = 5f;
    //Usually want to match original vcam setting for zoom out size
    public float zoomOutSize = 10f;
    public float transitionSpeed = 5f;
    public bool stopFollowingPlayer = false;
    GameObject focusPoint;
    static int focusPointCounter = 0;
    Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        focusPoint = new GameObject("CameraFocusPoint" + focusPointCounter);
        focusPointCounter++;
        virtualCamera = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CinemachineVirtualCamera>();
        confiner = virtualCamera.GetComponent<CinemachineConfiner>();
        roomCollider = this.GetComponent<PolygonCollider2D>();

        focusPoint.transform.position = roomCollider.bounds.center;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if (collision.CompareTag("Player"))
        {
            confiner.m_BoundingShape2D = roomCollider;
            if (stopFollowingPlayer)
            {
                StartCoroutine(ZoomIn());
                //Zoom in to confiner and stop following player
                confiner.enabled = true;
                confiner.m_ConfineMode = CinemachineConfiner.Mode.Confine2D;
                virtualCamera.Follow = null;
                virtualCamera.Follow = focusPoint.transform;
                
            }
            else
            {
                StartCoroutine(ZoomIn());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {       
        if (collision.CompareTag("Player"))
        {
            confiner.m_BoundingShape2D = null;
            confiner.enabled = false;
            StartCoroutine(ZoomOut());
            virtualCamera.Follow = playerTransform;
        }
    }

    IEnumerator ZoomIn()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(zoomOutSize, zoomInSize, t);
            yield return null;
        }
    }

    IEnumerator ZoomOut()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(zoomInSize, zoomOutSize, t);
            yield return null;
        }
    }
}
