using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldForceField : MonoBehaviour
{
    public float force, rotationSpeed, toPlayerDistance, toPlayerSpeed, pushForce, maxMass;
    public PlayerControler playerControler;
    Rigidbody rb;
    List<Rigidbody> attachedRigidbodys;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        attachedRigidbodys = new List<Rigidbody>();
    }
    void FixedUpdate()
    {
        foreach (Rigidbody r in attachedRigidbodys)
        {
            if (r != null)
            {
                r.velocity = (rb.position - r.position) * force;
                r.angularVelocity = Vector3.one * rotationSpeed;
            }
        }
        rb.position = playerControler.holdDefaultPosition;
        rb.velocity = (rb.position - playerControler.holdDefaultPosition) * toPlayerSpeed;
    }
    

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer != 10 && collider?.attachedRigidbody?.mass < maxMass)
        {
            attachedRigidbodys.Add(collider.attachedRigidbody);
        }
    }

    public void Push()
    {
        foreach (Rigidbody r in attachedRigidbodys)
        {
            if (r != null)
            {
                r.AddForce((rb.position - playerControler.transform.position) * pushForce);
            }
        }
        attachedRigidbodys?.Clear();
        gameObject.SetActive(false);
    }
}
