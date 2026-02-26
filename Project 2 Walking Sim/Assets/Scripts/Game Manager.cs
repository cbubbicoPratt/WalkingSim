using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //static: a variable that belongs to the class itself rather than a specific instance of that class
    public static GameManager instance;
    
    void Awake()
    {
        //if we dont have a game manager in the next scene then dont destroy this one
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //if there is a game manager in the new scene this one is destroyed
            Destroy(gameObject);
        }
    }

    
    void Update()
    {
        
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //if we click this button the scene will be reloaded
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Reloaded!");
        }
    }
}
