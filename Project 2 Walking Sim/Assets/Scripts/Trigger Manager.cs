using System;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    //listen for dialogue interactable check trigger, compare here
    //if the same, set active if not don't

    public static event Action<bool> OnActive;
    public static void UpdateActive(bool active)
    {
        OnActive?.Invoke(active);
    }
}
