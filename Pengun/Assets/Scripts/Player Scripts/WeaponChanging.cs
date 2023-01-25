using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanging : MonoBehaviour
{
    public CassetteTapes cassetteTapes;
    public TimeManager timemanager;
    bool hasTapeChanged1, hasTapeChanged2, hasTapeChanged3 = false;

    private void Update()
    {
        ManageCassetteTapes();
        OpenRadialMenu();
    }

    private void ManageCassetteTapes()
    {
        if (RadialMenuScript.selection == 2 && !hasTapeChanged1)
        {
            cassetteTapes.ChangeToBaseTrackUp();
            ResetHasTrackedChanged(true, false, false);
        }
        else if (RadialMenuScript.selection == 1 && !hasTapeChanged2)
        {
            cassetteTapes.ChangeToTrackRight();
            ResetHasTrackedChanged(false, true, false);
        }
        else if (RadialMenuScript.selection == 0 && !hasTapeChanged3)
        {
            cassetteTapes.ChangeToTrackLeft();
            ResetHasTrackedChanged(false, false, true);
        }

    }


    bool isNotInMenu = false;

    void OpenRadialMenu()
    {
        if (Input.GetButton("OpenAbilityMenu") && PlayerMovement.canMove)
        {
            timemanager.StartSlowMotion(0.1f);
            RadialMenuScript.isActive = true;
            isNotInMenu = false;
            PlayerMovement.canUseButtonInput = false;
        }
        else if (isNotInMenu == false)
        {
            timemanager.StopSlowMotion();
            RadialMenuScript.isActive = false;
            isNotInMenu = true;
            PlayerMovement.canUseButtonInput = true;
        }
        else
        {
            return;
        }
    }

    void ResetHasTrackedChanged(bool track1, bool track2, bool track3)
    {
        hasTapeChanged1 = track1;
        hasTapeChanged2 = track2;
        hasTapeChanged3 = track3;
    }
}
