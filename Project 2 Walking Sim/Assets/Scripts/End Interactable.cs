using JetBrains.Annotations;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EndInteractable : Interactable
{
    //class similar to dialogue interactable
    //this one ends the game
    //only on one object

    public string trigger;
    private bool isActive = false;
    private bool startFade = false;
    private bool faded = false;
    public GameObject sprite;
    private GameObject instantiated;

    public UnityEngine.UI.Image screen;

    private void OnEnable()
    {
        CCPlayer.OnTrigger += CheckTrigger;
    }

    private void Awake()
    {
        //again we need to set target object false through script after getting it
        screen.gameObject.SetActive(false);
    }
    private void Update()
    {
        bool isInstantiated = false;
        if (isActive && !isInstantiated)
        {
            instantiated = Instantiate(sprite, transform.position + Vector3.up, Quaternion.identity);
            isInstantiated = true;
        }
        CheckFaded(faded);
        if(startFade && !faded)
        {
            StartCoroutine(FadeOut(15));
        }
    }

    //function to process fading
    public void CheckFaded(bool fade)
    {
        if (fade)
        {
            SceneManager.LoadScene(2);
        }
    }

    //interacting sets fade in motion
    public override void Interact(CCPlayer ccplayer)
    {
        if(isActive)
        {
            Destroy(instantiated);
            startFade = true;
        }
    }

    
    public override bool BroadcastActive()
    {
        return isActive;
    }

    //same trigger script to set active
    //no listener because we only need it for this one object
    public void CheckTrigger(string str)
    {
        if (trigger == null)
        {
            Debug.Log("No trigger!");

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
                SetActive(true);
            }
            else
            {
                Debug.Log("Strings are not the same " + str + trigger);
            }
        }

    }
    public void SetActive(bool isOn)
    {
        isActive = isOn;
    }

    //coroutine to fade out
    private IEnumerator FadeOut(float duration)
    {
        Color startColor = new Color(screen.color.r, screen.color.g, screen.color.b, 0);
        Color targetColor = new Color(screen.color.r, screen.color.g, screen.color.b, 100);

        screen.gameObject.SetActive(true);
        yield return Fade(startColor, targetColor, duration);
    }

    //coroutine handling fading
    private IEnumerator Fade(Color startColor, Color targetColor, float duration)
    {
        float elapsedTime = 0;
        float elapsedPercentage = 0;

        while (elapsedTime < 1)
        {
            elapsedPercentage = elapsedTime / duration;
            screen.color = Color.Lerp(startColor, targetColor, elapsedPercentage);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
        faded = true;
    }
}
