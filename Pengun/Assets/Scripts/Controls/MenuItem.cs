using UnityEngine.UI;
using UnityEngine;

public class MenuItem : MonoBehaviour
{
    public Color hoverColor;
    public Color baseColor;
    public Image background;
    public Image icon;

    private void Start()
    {
        background.color = baseColor;
    }

    public void Select()
    {
        background.color = hoverColor;
        icon.rectTransform.localScale = new Vector3(0.11f,0.11f,0.11f);
    }

    public void Deselect()
    {
        background.color = baseColor;
        icon.rectTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
}
