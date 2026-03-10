using TMPro;
using UnityEditor;
using UnityEngine;

public class DialogueInteractable : Interactable
{
    private static GameObject toolTip;
    public GameObject sprite;
    public DialogueData dialogueData;
    private GameObject instantiated;

    private void Awake()
    {
        //find the tooltip and set it to inactive (won't find if already inactive
        toolTip = GameObject.FindGameObjectWithTag("Tooltip");
        toolTip.SetActive(false);
    }

    private void Start()
    {
        //make an instance of the triangle sprite to appear over the interactable
        //if (sprite != null) Debug.Log("Found Sprite: " + sprite.name);
        instantiated = Instantiate(sprite, transform.position + Vector3.up, Quaternion.identity);
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
        //after interacting, remove the sprite permanently
        Destroy(instantiated);
        if(dialogueData == null)
        {
            Debug.Log("npc has no data: " + gameObject.name);
        }
        ShowTooltip(true);
        //if we are interacting with the npc and it has data then request dialogue
        ccplayer.RequestDialogue(dialogueData);
    }
}
