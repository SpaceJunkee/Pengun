using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{

    public VolumeProfile profile;
    public static ChromaticAberration myChromaticAberration;
    public static Vignette myVignette;

    private void Start()
    {
        profile.TryGet(out myChromaticAberration);
        profile.TryGet(out myVignette);
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
