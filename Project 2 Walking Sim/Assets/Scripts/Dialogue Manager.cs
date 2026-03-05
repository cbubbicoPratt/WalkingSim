using TMPro;
using UnityEngine;

public class DialogueManage : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    public string master = "Welcome!";

    private void Start()
    {
        displayText.text = master;
    }

    private void OnEnable()
    {
        CCPlayer.OnDialogueRequested += StartDialogue;
    }

    private void OnDisable()
    {
        CCPlayer.OnDialogueRequested -= StartDialogue;
    }
    void StartDialogue(DialogueData dialogueData)
    {
        if (dialogueData == null)
        {
            Debug.Log("Missing NPC Data");
            return;
        }

        master += dialogueData.dialogue;
        
        if (displayText != null) displayText.text = master;
        Debug.Log($"Dialogue: {dialogueData.dialogue}");
    }
}
