using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControler : MonoBehaviour
{

    public float speed = 5, rotateSpeed, toHoldDistance;
    public Camera cam;
    public ForceField force;
    public HoldForceField holdForceField;
    public Image redScreen;
    public RectTransform winBlock, loseBlock, buttonBlock;
    public float joypadRadius = 200, forceHoldDelay = 0.5f, maxHealth;
    float health;


    Rigidbody rg;
    Transform viewAim;
    bool isMoving, isForceHold, isDead;
    Vector3 forceStart, moveStartPoint, currentMovePoint, lastOnScreenPosition;
    float holdTimer;

    void Start()
    {
        rg = GetComponent<Rigidbody>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Movement();
            Swipe();
            Color clr = redScreen.color;
            clr.a = 0.8f - 0.8f * Mathf.Clamp(health / maxHealth, 0, 1);
            redScreen.color = clr;
            health += Time.deltaTime / 5;
        }
    }
    void Movement()
    {
        float v = Input.GetAxis("Vertical"), h = Input.GetAxis("Horizontal");
        if (isMoving && v == 0 && h == 0)
        {
            Vector3 delta = (currentMovePoint - moveStartPoint) / joypadRadius;
            v = Mathf.Clamp(delta.y, -1, 1);
            h = Mathf.Clamp(delta.x, -1, 1);
        }

        rg.velocity = transform.forward * v * speed + transform.right * h * speed;
        Quaternion oldAngles = transform.rotation;
        transform.LookAt(viewAim);
        Quaternion angles = Quaternion.RotateTowards(oldAngles, transform.rotation, rotateSpeed);
        transform.rotation = Quaternion.Euler(0, angles.eulerAngles.y, 0);
    }
    void Swipe()
    {
        List<Touch> touches = SimulateTouch.GetTouches();
        if (touches.Count == 1)
        {
            Touch touch = touches[0];
            RaycastHit rch;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Physics.Raycast(cam.ScreenPointToRay(touch.position), out rch);
                    forceStart = rch.point;
                    holdTimer = forceHoldDelay;
                    isForceHold = false;
                    lastOnScreenPosition = touch.position;
                    break;
                case TouchPhase.Moved:
                    lastOnScreenPosition = touch.position;
                    break;
                case TouchPhase.Ended:
                    isMoving = false;
                    if (isForceHold)
                    {
                        holdForceField.Push();
                    }
                    else
                    {
                        holdForceField.gameObject.SetActive(false);
                        force.gameObject.SetActive(true);
                        force.transform.position = forceStart;
                        Physics.Raycast(cam.ScreenPointToRay(touch.position), out rch);
                        force.SetTarget(rch.point);
                        
                    }
                    break;
            }
            holdTimer -= Time.deltaTime;
            if (holdTimer <= 0 && !isForceHold)
            {
                isForceHold = true;
                holdForceField.gameObject.SetActive(true);
                Physics.Raycast(cam.ScreenPointToRay(touch.position), out rch);
                holdForceField.transform.position = rch.point;
                forceStart = holdDefaultPosition;
            }
        }

        if (touches.Count > 1)
        {
            Touch touch = touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    moveStartPoint = touch.position;
                    currentMovePoint = moveStartPoint;
                    isMoving = true;
                    break;
                case TouchPhase.Moved:
                    currentMovePoint = touch.position;
                    break;
                case TouchPhase.Ended:
                    isMoving = false;
                    break;
            }

        }
    }
    public void SetView(Transform tr)
    {
        viewAim = tr;
    }

    public Vector3 holdDefaultPosition
    {
        get { return transform.position + cam.ScreenPointToRay(lastOnScreenPosition).direction * toHoldDistance; }
    }

    public void Hit(float dmg = 1)
    {
        if (!isDead)
        {
            health -= dmg;
            if (health <= 0)
            {
                Defeat();
            }
        }
    }

    void Defeat()
    {
        isDead = true;
        loseBlock.gameObject.SetActive(true);
        buttonBlock.gameObject.SetActive(true);
    }
    public void Win()
    {
        isDead = true;
        winBlock.gameObject.SetActive(true);
        buttonBlock.gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
