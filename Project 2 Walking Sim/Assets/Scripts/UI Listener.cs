using TMPro;
using UnityEngine;

public class Listener : MonoBehaviour
{
    public TextMeshProUGUI statusText;

    public void OnEnable()
    {
        ButtonEvent.onButtonPressed += UpdateText;
    }

    public void OnDisable()
    {
        ButtonEvent.onButtonPressed -= UpdateText;
    }
    void UpdateText()
    {
        statusText.text = "Button Pressed";
    }

    //button didn't know text exists, but now is "listening"
}
