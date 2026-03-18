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
    public string triggerID;
    public string trigger;
    private bool isActive;
    
    private TextMeshProUGUI toolTip;
    public GameObject sprite;
    public DialogueData dialogueData;
    private GameObject instantiated;
    private bool isInstantiated = false;

    private void OnEnable()
    {
        CCPlayer.OnTrigger += CheckTrigger;
        TriggerManager.OnActive += SetActive;
    }

    //private void OnDisable()
    //{
        
    //}

    private void Awake()
    {
        //find the tooltip and set it to inactive (won't find if already inactive)
        toolTip = GameObject.Find("Tooltip").GetComponent<TextMeshProUGUI>();
        //Debug.Log("Found" + toolTip);
        
        //this is controlling on whether or not it is activee. 
        //not active = no dialogue 
        if(string.IsNullOrEmpty(trigger))
        {
            isActive = true;
        } 
        //even with unactive you can compare the two trigger strings as long as you press the interact key
        //but then i think it makes it really hard to know what to interact with but i also dont know why youre building this system
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

    public void SetActive(bool isOn)
    {
        isActive = isOn;
    }

    public override bool BroadcastActive()
    {
        bool isItActive = isActive;
        //Debug.Log("is it active?" +isItActive);
        return isActive;
    }

    //the problem was you made this function static which i didnt catch
    public void ShowTooltip(bool show)
    {
        //only shows tooltip if we find new dialogue
        if (show)
        {
            //to access the actual text mesh pro component instead of the game object use .enabled
            //so that means the Tooptip is active in the inspector and i just check off the actual text component
            toolTip.enabled = true;
            
        }
        else
        {
            toolTip.enabled = false;
        }
    }
    public override void Interact(CCPlayer ccplayer)
    {
        if (isActive)
        {
            if (!string.IsNullOrEmpty(triggerID))
            {
                ccplayer.BroadcastTrigger(triggerID);
            }
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
            //but we don't want it to disappear so we just set the script to inactive
            isActive = false;
            
            
        }
        else if (!isActive)
        {
            //you can also just call check trigger here and it works just make sure to turn off OnEnable
            
            //CheckTrigger(trigger);
            //Debug.Log("check trigger");
        }
        
        
        
    }

    public void CheckTrigger(string str)
    {
        if (dialogueData == null)
        {
            Debug.Log("No dialogue data!");
            
        }
        else
        {
            Debug.Log("have data on check trigger");
        }
        if (!string.IsNullOrEmpty(dialogueData.trigger))
        {
            Debug.Log("dialogue data trigger not empty " + dialogueData.trigger);
            if(str == dialogueData.trigger)
            {
                Debug.Log("Strings are the same " + str + dialogueData.trigger);
                TriggerManager.UpdateActive(true);
            }
            else
            {
                Debug.Log("Strings are not the same " + str + dialogueData.trigger);
            }
        }
        
    }
}

