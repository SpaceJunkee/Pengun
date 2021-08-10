using UnityEngine.UI;
using UnityEngine;

public class MenuItem : MonoBehaviour
{
    public Color hoverColor;
    public Color baseColor;
    public Image background;

    private void Start()
    {
        background.color = baseColor;
    }

    public void Select()
    {
        background.color = hoverColor;
    }

    public void Deselect()
    {
        background.color = baseColor;
    }
}
