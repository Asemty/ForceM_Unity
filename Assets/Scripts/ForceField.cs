using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    public float force, speed, maxLiveTime;
    float liveTime;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        rb.velocity = transform.forward * speed;
        liveTime -= Time.deltaTime;
        if (liveTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer != 10)
        {
            collider?.attachedRigidbody?.AddForce(rb.velocity * force);
        }
    }

    public void SetTarget(Vector3 target)
    {
        transform.LookAt(target);
        liveTime = maxLiveTime;
    }
}
