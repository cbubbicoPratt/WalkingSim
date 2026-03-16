using UnityEngine;

public class TriggerScript : Interactable
{
    public string triggerID;
    public bool isActive;

    public override void Interact(CCPlayer ccplayer)
    {
        Debug.Log("Calling interact on trigger");
        ccplayer.BroadcastTrigger(triggerID);
    }

    public override bool BroadcastActive()
    {
        bool isItActive = isActive;
        Debug.Log("is it active?" +isItActive);
        return isActive;
       
    }
}
