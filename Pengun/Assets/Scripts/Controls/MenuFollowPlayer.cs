using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFollowPlayer : MonoBehaviour
{
    public RectTransform movingObject;
    public Vector3 offset;
    public RectTransform menuObject;
    public Camera cam;
    public Vector3 playerPosition;

    private void Update()
    {
        playerPosition = this.GetComponentInParent<Transform>().position;
        MoveMenu();
    }

    public void MoveMenu()
    {
        Vector3 pos = playerPosition;
        pos.z = menuObject.position.z;
        movingObject.position = cam.ScreenToWorldPoint(pos);
    }
}
