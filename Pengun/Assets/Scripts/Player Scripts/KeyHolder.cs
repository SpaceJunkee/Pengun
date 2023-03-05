using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    bool isHoldingKey = false;
    GameObject keyGameObject;


    public bool getIsHoldingKey()
    {
        return isHoldingKey;
    }

    public void setIsHoldingKey(bool newHoldingKey)
    {
        isHoldingKey = newHoldingKey;
    }

    public void setKeyGameObject(GameObject newKeyGameObject)
    {
        keyGameObject = newKeyGameObject;
    }

    public GameObject getKeyGameObject()
    {
        return keyGameObject;
    }
}
