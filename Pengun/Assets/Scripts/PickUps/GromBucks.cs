using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GromBucks : MonoBehaviour
{

    /*Need to incorporate some way of saving grom currecny when exiting game*/

    TextMeshProUGUI gromBucksText;
    int numberOfGromBucks;
    string prefKey = "UpdateGromBucks";

    private void Start()
    {
        gromBucksText = GetComponentInChildren<TextMeshProUGUI>();
        numberOfGromBucks = PlayerPrefs.GetInt(prefKey);
    }

    private void Update()
    {
        
        gromBucksText.text = numberOfGromBucks.ToString();
    }

    public void AddToGromBucks(int numOfGromBucks)
    {
        numberOfGromBucks += numOfGromBucks;
        PlayerPrefs.SetInt(prefKey, numberOfGromBucks);
    }

    public int GetNumberOfGromBucks()
    {
        return numberOfGromBucks;
    }
}
