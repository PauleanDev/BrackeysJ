using UnityEngine;
using UnityEngine.UI;

public class ButtonAnim : MonoBehaviour
{
    private Text buttonText;
    private Color originalColor;

    private void Awake()
    {
        buttonText = GetComponentInChildren<Text>();
        originalColor = buttonText.color;
    }

    public void Select()
    {
        buttonText.color = Color.white;
    }

    public void Deselect()
    {
        buttonText.color = originalColor;
    }

}
