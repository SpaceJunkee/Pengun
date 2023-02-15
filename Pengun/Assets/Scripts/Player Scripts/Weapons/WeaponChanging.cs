using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanging : MonoBehaviour
{
    public TimeManager timemanager;
    PlayerAudioManager playerAudioManager;
    Shooting shooting;
    bool hasTapeChanged1, hasTapeChanged2, hasTapeChanged3 = false;

    private void Start()
    {
        playerAudioManager = GetComponent<PlayerAudioManager>();
        shooting = GetComponent<Shooting>();
    }

    private void Update()
    {
        ManageWeaponChanging();
        OpenRadialMenu();
    }

    private void ManageWeaponChanging()
    {
        if (RadialMenuScript.selection == 2 && !hasTapeChanged1)
        {
            //top
            ResetHasWeaponChanged(true, false, false);
            Debug.Log("Pistol");
            SwapToPistol();
            shooting.SelectActiveWeapon("Pistol");
            shooting.SelectActiveAltFire("PistolAlt");
        }
        else if (RadialMenuScript.selection == 1 && !hasTapeChanged2)
        {
            //right
            ResetHasWeaponChanged(false, true, false);
            Debug.Log("Machine Gun");
            SwapToMachineGun();
            shooting.SelectActiveWeapon("MachineGun");
            shooting.SelectActiveAltFire("MachineGunAlt");
        }
        else if (RadialMenuScript.selection == 0 && !hasTapeChanged3)
        {
            //left
            ResetHasWeaponChanged(false, false, true);
            Debug.Log("Shotgun");
            SwapToShotgun();
            shooting.SelectActiveWeapon("Shotgun");
            shooting.SelectActiveAltFire("ShotgunAlt");
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

    void ResetHasWeaponChanged(bool track1, bool track2, bool track3)
    {
        hasTapeChanged1 = track1;
        hasTapeChanged2 = track2;
        hasTapeChanged3 = track3;
    }

    private void SwapToShotgun()
    {
        //Play animation
    }

    private void SwapToMachineGun()
    {
        //Play animation
    }

    private void SwapToPistol()
    {
        //Play animation
    }
}
