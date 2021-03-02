using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainGoo : MonoBehaviour
{
    public Animator anim;

    private void Update()
    {
        if(Button.isButtonPushed == true)
        {
            anim.SetBool("hasGooDrained", true);
        }
    }
}
