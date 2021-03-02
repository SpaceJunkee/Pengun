using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{

    public Animator anim;
    public static bool isButtonPushed = false;

    public void ButtonPressed()
    {
        anim.SetBool("hasButtonBeenShot", true);
        isButtonPushed = true;
    }
}
