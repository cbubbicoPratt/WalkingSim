using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Listener : MonoBehaviour
{
    public Object scene;

    public void OnEnable()
    {
        ButtonEvent.onButtonPressed += UpdateScene;
    }

    public void OnDisable()
    {
        ButtonEvent.onButtonPressed -= UpdateScene;
    }
    void UpdateScene()
    {
        SceneManager.LoadScene(scene.name);
    }
}
