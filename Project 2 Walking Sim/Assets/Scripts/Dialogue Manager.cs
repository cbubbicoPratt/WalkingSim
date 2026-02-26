using TMPro;
using UnityEngine;

public class DialogueManage : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI placeholderOpeningLine;

    private void OnEnable()
    {
        CCPlayer.OnDialogueRequested += StartDialogue;
    }

    private void OnDisable()
    {
        CCPlayer.OnDialogueRequested -= StartDialogue;
    }
    void StartDialogue(NPCData npcData)
    {
        if (npcData == null)
        {
            Debug.Log("Missing NPC Data");
            return;
        }

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (displayName != null) displayName.text = npcData.displayName;
        if (placeholderOpeningLine != null) placeholderOpeningLine.text = npcData.placeHolderOpeningLine;
        Debug.Log($"Dialogue start with {npcData.displayName}: {npcData.placeHolderOpeningLine}")
    }
}
