using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMind : MonoBehaviour
{
    public PlayerControler playerControler;
    public Transform ragdoll;
    public float alarmDistance, alarmCollisionSpeed, deadCollisionSpeed, hitDistance, maxHitTimer;
    public Animator animator;
    public SkinnedMeshRenderer render;
    public Material whiteMaterial;

    Rigidbody[] ragdollParts;
    Rigidbody rb;
    bool isAlarmed, isAlive = true;
    NavMeshAgent meshAgent;
    float hitTimer;
    void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        ragdollParts = ragdoll.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in ragdollParts)
        {
            r.GetComponent<Collider>().enabled = false;
            r.mass = 5;
            r.isKinematic = true;
            //r.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (Vector3.Distance(transform.position, playerControler.transform.position) < alarmDistance)
            {
                Alarm();
            }
            if (isAlarmed)
            {
                meshAgent.SetDestination(playerControler.transform.position);
            }
            if (Vector3.Distance(transform.position, playerControler.transform.position) < hitDistance)
            {
                HitPlayer();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.relativeVelocity.magnitude > deadCollisionSpeed)
        {
            Dead();
            return;
        }
        if (collision.relativeVelocity.magnitude > alarmCollisionSpeed)
        {
            Alarm();
            return;
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Force"))
        {
            Dead();
            return;
        }
    }
    void Dead()
    {
        isAlive = false;
        Destroy(GetComponent<BoxCollider>());
        foreach (Rigidbody r in ragdollParts)
        {
            //r.useGravity = true;
            r.GetComponent<Collider>().enabled = true;
            r.isKinematic = false;
        }
        Destroy(animator);
        Destroy(meshAgent);
        render.material = whiteMaterial;
    }
    void Alarm()
    {
        isAlarmed = true;
        animator.SetBool("run", true);
    }
    void HitPlayer()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer >= maxHitTimer)
        {
            hitTimer -= maxHitTimer;
            playerControler.Hit();
        }
    }
}
