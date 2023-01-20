using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeStation : MonoBehaviour
{
public float interactDistance = 2f;
    public GameObject upgradeMenuUI;
    private GameObject player;
    PlayerMovement playerMovement;
    public bool isInRange = false;
    public bool isInMenu = false;
    public bool isNotInMenu = true;
    float distance;
    public float rangeRadius;
    public bool isInMenuNavigation = false;
    public Image[] upgradeImages;
    int currentImageIndex = 0;

    public bool inputProcessedHorizontal = false;
    public bool inputProcessedVertical = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        upgradeMenuUI.SetActive(false);
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Debug.Log(currentImageIndex);
        Debug.Log(Input.GetAxis("HorizontalNav") + " " + Input.GetAxis("VerticalNav"));
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

        NavigateUpgradeMenu();
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
        playerMovement.StopPlayer(true, false, true);
        isInMenuNavigation = true;
    }

    void CloseUpgradeMenu()
    {
        upgradeMenuUI.SetActive(false);
        isInMenu = false;
        isNotInMenu = true;
        playerMovement.EnableMovement();
        isInMenuNavigation = false;
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

    private void NavigateUpgradeMenu()
    {

        ResetNavImageIfOutOfIndex();

        if (isInMenuNavigation)
        {
            if (Input.GetAxis("HorizontalNav") != 0)
            {
                int horizontalValue = (int)Input.GetAxis("HorizontalNav");
                if (!inputProcessedHorizontal)
                {
                    currentImageIndex += horizontalValue;
                    inputProcessedHorizontal = true;
                }
            }

            if (Input.GetAxis("VerticalNav") !=0)
            {
                int verticalValue = (int)Input.GetAxis("VerticalNav");
                if (!inputProcessedVertical)
                {
                    if (verticalValue > 0)
                    {
                        currentImageIndex -= 5; // move to the previous row
                    }
                    else if (verticalValue < 0)
                    {
                        currentImageIndex += 5; // move to the next row
                    }
                    inputProcessedVertical = true;
                }
            }

            if (Input.GetAxis("HorizontalNav") == 0)
            {
                inputProcessedHorizontal = false;
            }

            if (Input.GetAxis("VerticalNav") == 0)
            {
                inputProcessedVertical = false;
            }
        }

        for (int i = 0; i < upgradeImages.Length; i++)
        {
            if (i == currentImageIndex)
            {
                upgradeImages[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                upgradeImages[i].color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
        }

    }

    private void ResetNavImageIfOutOfIndex()
    {
        if (currentImageIndex < 0)
            currentImageIndex = 9;
        if (currentImageIndex >= 10)
            currentImageIndex = 0;
    }
}
