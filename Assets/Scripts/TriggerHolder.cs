using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHolder : MonoBehaviour
{
    public PlayerControler controler;
    CurrentTrigger[] triggers;
    void Start()
    {
        triggers = GetComponentsInChildren<CurrentTrigger>();
        controler.SetView(triggers[0].transform);
    }

    public void TriggerUpdate(CurrentTrigger ct)
    {
        for(int i = 0; i < triggers.Length - 1; i++)
        {
            if (triggers[i] == ct)
            {
                controler.SetView(triggers[i + 1].transform);
                return;
            }
        }
        if(triggers[triggers.Length - 1] == ct)
        {
            controler.Win();
        }
    }
}
