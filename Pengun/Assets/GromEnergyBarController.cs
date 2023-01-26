using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GromEnergyBarController : MonoBehaviour
{
    public float currentGromEnergy;
    public float maxGromEnergy;
    public float minGromEnergy;
    public Image fillImage;
    public bool isInTestingMode = false;

    private void Start()
    {
        //Set current grom energy to whatever it is at save point
    }

    private void Update()
    {
        if (isInTestingMode)
        {
            maxGromEnergy = 100000f;
            currentGromEnergy = 100000f;
            minGromEnergy = 100000f;
        }
        CheckIfCurrentIsMinOrMax();
    }

    public void IncreaseGromEnergy(int amount)
    {
        currentGromEnergy += amount;

        UpdateGromEnergy();
    }

    public void DecreaseGromEnergy(float amount)
    {
        currentGromEnergy -= amount;

        UpdateGromEnergy();
    }


    public void UpdateGromEnergy()
    {
        fillImage.fillAmount = currentGromEnergy / maxGromEnergy;
    }

    void CheckIfCurrentIsMinOrMax()
    {
        if(currentGromEnergy >= maxGromEnergy)
        {
            currentGromEnergy = maxGromEnergy;
        }
        else if(currentGromEnergy <= minGromEnergy)
        {
            currentGromEnergy = minGromEnergy;
        }
    }
}
