using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    //controls the text panel and dialogue for the whole game
    public GameObject panel;
    public TextMeshProUGUI dialogue;

    //checks if we opened menu
    public void ToggleMenu(bool isActive)
    {
        panel.SetActive(isActive);
    }
}
