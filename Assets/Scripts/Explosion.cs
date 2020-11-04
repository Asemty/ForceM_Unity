using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float force, radius, liveTime = 0.5f;
    public Transform body;

    Vector3 sizeStep;

    void Start()
    {
        sizeStep = Vector3.one * ((radius - 1) / liveTime);
    }
    void Update()
    {
        //todo скорость роста зависит от delta time
        body.localScale = body.localScale + sizeStep * Time.deltaTime;
        if (body.localScale.x > radius * 2)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(collider);

        collider?.attachedRigidbody?.AddExplosionForce(force, transform.position, radius);
    }
}
