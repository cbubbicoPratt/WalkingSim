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
        bob.y = Mathf.Sin(Time.time * 5f) * 0.0015f;
        transform.position += bob;
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}
