using System;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    //listen for dialogue interactable check trigger, compare here
    //if the same, set active if not don't

    public DialogueData dialogueData;
    private DialogueInteractable dialogue;
    private string trigger;

    private void Start()
    {
        trigger = dialogueData.trigger;
        dialogue = GetComponent<DialogueInteractable>();
    }

    private void OnEnable()
    {
        CCPlayer.OnTrigger += CheckTrigger;
    }

    public static event Action<bool> OnActive;
    public static void UpdateActive(bool active)
    {
        OnActive?.Invoke(active);
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
        if (!string.IsNullOrEmpty(trigger))
        {
            Debug.Log("dialogue data trigger not empty " + trigger);
            if (str == trigger)
            {
                Debug.Log("Strings are the same " + str + trigger);
                dialogue.SetActive(true);
            }
            else
            {
                Debug.Log("Strings are not the same " + str + trigger);
            }
        }
    }
}
