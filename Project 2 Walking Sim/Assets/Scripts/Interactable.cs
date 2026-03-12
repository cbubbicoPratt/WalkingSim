using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact(CCPlayer ccplayer);
    //function to show if our interactable script is active
    public abstract bool BroadcastActive();
}
