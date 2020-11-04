using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBarrel : MonoBehaviour
{
    public float explosionSpeed = 10;
    public GameObject explosion;
    Rigidbody rb;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > explosionSpeed)
        {
            Explode();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Explode()
    {
        Destroy(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
