using TMPro;
using UnityEngine;

public class DialogueInteractable : Interactable
{
    private static GameObject toolTip;
    public DialogueData dialogueData;

    private void Awake()
    {
        //find the tooltip and set it to inactive (won't find if already inactive
        toolTip = GameObject.FindGameObjectWithTag("Tooltip");
        toolTip.SetActive(false);
    }

    public static void ShowTooltip(bool show)
    {
        //only shows tooltip if we find new dialogue
        if (show)
        {
            toolTip.SetActive(true);
        }
        else
        {
            toolTip.SetActive(false);
        }
    }
    public override void Interact(CCPlayer ccplayer)
    {
        if(dialogueData == null)
        {
            Debug.Log("npc has no data: " + gameObject.name);
        }
        ShowTooltip(true);
        //if we are interacting with the npc and it has data then request dialogue
        ccplayer.RequestDialogue(dialogueData);
    }
}
