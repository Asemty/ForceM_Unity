using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<PlayerControler>() != null)
        {
            transform.parent.GetComponent<TriggerHolder>().TriggerUpdate(this);
        }
    }
}
