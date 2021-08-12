using UnityEngine.UI;
using UnityEngine;

public class RadialMenuScript : MonoBehaviour
{

    public Vector2 normalisedPosition;
    public float currentAngle;
    public float previousAngle;
    public static int selection;
    private int previousSelection;

    public static bool isActive = false;
    bool hasMovedStick = false;

    public GameObject[] menuItems;

    private MenuItem menuItem;
    private MenuItem previousMenuItem;

    private void Start()
    {
        selection = 3;
    }

    private void Update()
    {

        if(Input.GetAxis("RightStickX") >0 || Input.GetAxis("RightStickY") > 0 || Input.GetAxis("RightStickX") < 0 || Input.GetAxis("RightStickY") < 0)
        {
            hasMovedStick = true;
        }

        if (isActive)
        {
            for (int i =0; i < menuItems.Length; i++)
            {
                menuItems[i].SetActive(true);
            }

            if (hasMovedStick)
            {
                currentAngle = Angle(Input.GetAxis("RightStickX"), Input.GetAxis("RightStickY"));
                selection = (int)currentAngle / 90;
            }
                       

            if (selection != previousSelection)
            {
                previousMenuItem = menuItems[previousSelection].GetComponent<MenuItem>();
                previousMenuItem.Deselect();
                previousSelection = selection;

                menuItem = menuItems[selection].GetComponent<MenuItem>();
                menuItem.Select();
            }
        }
        else
        {
            for (int i = 0; i < menuItems.Length; i++)
            {
                menuItems[i].SetActive(false);
            }
        }
        
    }

    float Angle(float x, float y)
    {
        //we will use -1 to mean no input
        //otherwise return an angle between 0 and 360 inclusive
        float RefAngle = Mathf.Abs(Mathf.Rad2Deg * Mathf.Atan2(y, x));
        float MainAngle = 0;
        if (y == 0 && x == 0) MainAngle = currentAngle; //not pointing at anything, centered
        if (y < 0 && x != 0) MainAngle = 360 - RefAngle; //somewhere between 180 and 360 but not 270
        if (y > 0 && x != 0) MainAngle = RefAngle;  //somewhere between 0 and 180 but not 90;
        if (y == 0 && x > 0) MainAngle = 0; //pointing right
        if (y > 0 && x == 0) MainAngle = 90; //pointing up
        if (y == 0 && x < 0) MainAngle = 180; //pointing left
        if (y < 0 && x == 0) MainAngle = 270;  //pointing down
        return MainAngle;
    }


}


