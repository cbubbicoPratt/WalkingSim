using UnityEngine;

public class DestroyInteractable : Interactable
{
    public bool isActive;
    public override bool BroadcastActive()
    {
        return isActive;
    }
    public override void Interact(CCPlayer ccplayer)
    {
        Destroy(gameObject);
        Debug.Log("Destroyed " + gameObject.name + "!");
    }
}
