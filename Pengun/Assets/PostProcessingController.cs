using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{

    public VolumeProfile profile;
    public ChromaticAberration myChromaticAberration;

    private void Start()
    {
        profile.TryGet(out myChromaticAberration);
    }


    private void Update()
    {
        if (RadialMenuScript.isActive)
        {
            myChromaticAberration.active = true;
        }
        else
        {
            myChromaticAberration.active = false;
        }
    }
}
