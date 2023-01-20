using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GromBucks : MonoBehaviour
{

    /*Need to incorporate some way of saving grom currecny when exiting game*/

    Text gromBucksText;
    int numberOfGromBucks;

    private void Start()
    {
        gromBucksText = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        gromBucksText.text = numberOfGromBucks.ToString();
    }

    public void AddToGromBucks(int numOfGromBucks)
    {
        numberOfGromBucks += numOfGromBucks;
    }
}
