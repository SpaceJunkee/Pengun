using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStation : MonoBehaviour
{
public float interactDistance = 2f;
    public GameObject upgradeMenuUI;
    private GameObject player;
    public bool isInRange = false;
    public bool isInMenu = false;
    public bool isNotInMenu = true;
    float distance;
    public float rangeRadius;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        upgradeMenuUI.SetActive(false);
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        rangeRadius = interactDistance;

        if (distance <= interactDistance)
        {
            isInRange = true;
        }
        else
        {
            isInRange = false;
        }

        if (isInRange && Input.GetButtonDown("Melee"))
        {
            if (isNotInMenu)
            {
                OpenUpgradeMenu();
            }
            else if (isInMenu)
            {
                CloseUpgradeMenu();
            }
            
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {          
            if (distance <= interactDistance)
            {
                isInRange = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    void OpenUpgradeMenu()
    {
        upgradeMenuUI.SetActive(true);
        isInMenu = true;
        isNotInMenu = false;
}

    void CloseUpgradeMenu()
    {
        upgradeMenuUI.SetActive(false);
        isInMenu = false;
        isNotInMenu = true;
    }

    public void UpgradePlayer(string upgradeType)
    {
        switch(upgradeType)
        {
            case "Health":
                // Apply Health Upgrade
                break;
            case "Damage":
                // Apply Damage Upgrade
                break;
            case "Speed":
                // Apply Speed Upgrade
                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeRadius);
    }
}
