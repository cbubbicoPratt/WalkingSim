using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public string trigger;
    private bool moveUp = false;
    private float speed = 3;
    private Vector3 target;
    

    private void OnEnable()
    {
        CCPlayer.OnTrigger += CheckTrigger;
        TriggerManager.OnActive += MoveUp;
    }
    private void Awake()
    {
        target = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
    }
    private void Update()
    {
        float step = speed * Time.deltaTime;
        if(moveUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
    }

    public void MoveUp(bool isOn)
    {
        moveUp = isOn;
    }

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
                TriggerManager.UpdateActive(true);
            }
            else
            {
                Debug.Log("Strings are not the same " + str + trigger);
            }
        }

    }
}
