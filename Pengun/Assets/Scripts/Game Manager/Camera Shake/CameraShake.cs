using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    private float shakeTimer;
    public static CameraShake Instance { get;  set; }
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    bool isAlreadyShaking = false;

    private void Start()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }


    public void ShakeCamera(float shakeIntensity, float frequencyGain, float time)
    {
        if(isAlreadyShaking == false)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = shakeIntensity;
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequencyGain;
            shakeTimer = time;
            isAlreadyShaking = true;
        }
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
            isAlreadyShaking = false;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }
}
