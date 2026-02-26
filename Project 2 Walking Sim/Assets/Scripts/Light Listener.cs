using UnityEngine;

public class LightListener : MonoBehaviour
{
    //listener script is like twitter followers; alerts all 'subscribers'
    public Light sceneLight;

    //when the object becomes active it is saying when your event fires, call my change light function
    public void OnEnable()
    {
        ButtonEvent.onButtonPressed += ChangeLight;
    }

    //when disabled, remove ourselves from list
    public void OnDisable()
    {
        ButtonEvent.onButtonPressed -= ChangeLight;
    }

    void ChangeLight()
    {
        sceneLight.color = Random.ColorHSV();
    }
}
