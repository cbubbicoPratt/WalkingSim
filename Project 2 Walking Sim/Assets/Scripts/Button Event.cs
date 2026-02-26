using System;
using TMPro;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    //action is a delegate
    //a delegate is a variable that can store a function
    //int number vs Action myFunction
    //can be used by anyone
    //static belongs to the class itself not specific instance
    //we don't need reference to specific GameObject
    //instead of find object of type
    //one shared version across entire project

    //event is a special type of delegate
    //it is protected. if u do this without event it can break
    //other scripts can subscribe and unsubscribe but they cannot invoke it
    public static event Action onButtonPressed;

    public void OnButtonPressed()
    {
        //invoke means to call every function subscribed to this invent
        //"?." means only do this if it isn't null (if someone is listening)
        onButtonPressed?.Invoke();
    }



    //==========
    //EXAMPLE
    //==========

    //this is not scalable. if we want ten things to react when we press the button,
    //we would have ten references and have to keep changing this script
    //also if we want to delete something later it can't break this script
    //why should button be responsible for light and text?
    //private void PressMe()
    //{
    //Light light = GetComponent<Light>();
    //TextMeshProUGUI statusText = GetComponent<TextMeshProUGUI>();

    //light.color = Color.white;
    //statusText.text = "Pressed";

    //what if we want things to react when we press the button?
    //}
}
