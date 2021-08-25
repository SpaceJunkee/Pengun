using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    private float shakeTimer;
    public static CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }


    public void ShakeCamera(float shakeIntensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = shakeIntensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
        }
        
        if(shakeTimer <= 0f)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }
}
