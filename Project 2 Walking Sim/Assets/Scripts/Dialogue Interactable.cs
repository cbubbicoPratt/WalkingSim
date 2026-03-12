using TMPro;
using UnityEditor;
using UnityEngine;

public class DialogueInteractable : Interactable
{
    //if another dialogue object has a trigger attached to it upon interaction
    //broadcast that trigger when we interact with it
    //compare trigger in the same script to one set in inspector
    //if one isn't set, the object should be active by default
    //if the trigger is the same the object should be active


    //manually inputted string to detect if the interactable needs a trigger to activate
    public string trigger;
    private bool isActive;

    private static GameObject toolTip;
    public GameObject sprite;
    public DialogueData dialogueData;
    private GameObject instantiated;
    private bool isInstantiated = false;

    private void Awake()
    {
        //find the tooltip and set it to inactive (won't find if already inactive)
        toolTip = GameObject.FindGameObjectWithTag("Tooltip");
        toolTip.SetActive(false);
        if(trigger == null)
        {
            isActive = true;
        } 
        else
        {
            isActive = false;
        }
    }

    private void Update()
    {
        if(isActive && !isInstantiated)
        {
            instantiated = Instantiate(sprite, transform.position + Vector3.up, Quaternion.identity);
            isInstantiated = true;
        }
    }

    public override bool BroadcastActive()
    {
        return isActive;
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

    public void UpdateActive(string triggerWord)
    {
        if (triggerWord == null) return;
        if (triggerWord == trigger) isActive = true;
    }
    public override void Interact(CCPlayer ccplayer)
    {
        if (isActive)
        {
            //after interacting, remove the sprite permanently
            Destroy(instantiated);
            if (dialogueData == null)
            {
                Debug.Log("npc has no data: " + gameObject.name);
            }
            ShowTooltip(true);
            //if we are interacting with the npc and it has data then request dialogue
            ccplayer.RequestDialogue(dialogueData);
            if(dialogueData.trigger != null)
            {
                UpdateActive(dialogueData.trigger);
            }
            //we don't want the player interacting with this object again
            //but we don't want it to disappear so we just delete the script
            isActive = false;
        }
    }
}
