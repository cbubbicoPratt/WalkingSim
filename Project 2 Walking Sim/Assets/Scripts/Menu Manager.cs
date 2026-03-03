using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI dialogue;

    public void ToggleMenu(bool isActive)
    {
        panel.SetActive(isActive);
    }
}
