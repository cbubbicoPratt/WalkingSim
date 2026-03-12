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

    private static TextMeshPro toolTip;
    public GameObject sprite;
    public DialogueData dialogueData;
    private GameObject instantiated;
    private bool isInstantiated = false;

    private void OnEnable()
    {
        CCPlayer.OnTrigger += CheckTrigger;
    }

    //private void OnDisable()
    //{
        
    //}

    private void Awake()
    {
        //find the tooltip and set it to inactive (won't find if already inactive)
        toolTip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<TextMeshPro>();
        if(string.IsNullOrEmpty(trigger))
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
            toolTip.gameObject.SetActive(true);
        }
        else
        {
            toolTip.gameObject.SetActive(false);
        }
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
           
            //we don't want the player interacting with this object again
            //but we don't want it to disappear so we just delete the script
            isActive = false;
        }
    }

    public void CheckTrigger(string str)
    {
        if (dialogueData == null)
        {
            Debug.Log("No dialogue data!");
            return;
        }
        if (!string.IsNullOrEmpty(dialogueData.trigger))
        {
            if(str == trigger)
            {
                Debug.Log("Strings are the same");
            }
        }
    }
}
