using UnityEngine;

public class InteractableSpriteHandler : MonoBehaviour
{
    private Transform target;
    private Vector3 bob;
    void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

   
    void Update()
    {
        //bobbing motion created with tiny sine wave which is added to position
        bob.y = Mathf.Sin(Time.time * 5f) * 0.0015f;
        transform.position += bob;

        //make the sprite always face the player
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}
