// Script written by: Brooke Boster

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // The speed of the player and the move input!
    [SerializeField] private float speed;
    private Vector2 move;

    // Input and jazz.
    public PlayerInput PlayerInputInstance;
    public InputAction Dash;
    public InputAction Shoot;

    [SerializeField] private Rigidbody rb;

    // The camera, bullet prefab, the position where the bullet spawns, the helper (mouse position), and the shoot speed.
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootPoint;
    //[SerializeField] private Vector3 target;
    //[SerializeField] private GameObject crossHair;
    [SerializeField] private GameObject helper;
    [SerializeField] private float shootSpeed;

    // the ray to get the mouse position
    private Ray shootRay;

    [SerializeField] private float enemyDamage = 10f;

    public PlayerHealth PH;

    public AudioClip CrossbowSFX, DashSFX;
    private AudioSource source;

    // Gets the movement input from the InputActions action map.
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    private void Start()
    {
        // Getting the action map
        PlayerInputInstance = GetComponent<PlayerInput>();
        PlayerInputInstance.currentActionMap.Enable();

        // Getting the dash from the action map
        Dash = PlayerInputInstance.currentActionMap.FindAction("Dash");
        Shoot = PlayerInputInstance.currentActionMap.FindAction("Shoot");

        // Dash started and cancelled
        Dash.started += Dash_started;
        Dash.canceled += Dash_canceled;

        Shoot.started += Shoot_started;
        Shoot.canceled += Shoot_canceled; 

        // Get the rigidbody component
        rb = GetComponent<Rigidbody>();

        // Get the audiosource component
        source = GetComponent<AudioSource>();
    }

    private void Shoot_canceled(InputAction.CallbackContext obj)
    {
        
    }

    private void Shoot_started(InputAction.CallbackContext obj)
    {
        Shooting();
    }

    // Instantiate the bullet, get it's rigidbody, and add a force to it in the direction of the helper.
    public void Shooting()
    {
        source.PlayOneShot(CrossbowSFX);
        GameObject bullets = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        Rigidbody rb = bullets.GetComponent<Rigidbody>();
        Vector3 shootDirection = (helper.transform.position - shootPoint.position).normalized;
        rb.AddForce(shootDirection * shootSpeed);
        //rb.transform.position = Vector3.MoveTowards(shootPoint.position, helper.transform.position, shootSpeed * Time.deltaTime);
        //Vector3 mousePosition = Input.mousePosition;
        //rb.transform.position = helper.transform.position;
    }

    private void FixedUpdate()
    {
        // transform the helper to the ray gotten from the mouse position.
        shootRay = camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(shootRay, out RaycastHit raycastHit))
        {
            helper.transform.position = raycastHit.point;
        }
    }

    private void Dash_canceled(InputAction.CallbackContext obj)
    {
        Debug.Log("Dash stopped");
    }

    private void Dash_started(InputAction.CallbackContext context)
    {
        Debug.Log("Dash started");
        source.PlayOneShot(DashSFX);
        rb.AddForce(transform.forward * 1000);
    }

    // Currently just moves the player.
    void Update()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        // Get's a move x and move y from input action and translates the players movement.
        // Also has the player rotate.
        Vector3 movement = new Vector3(move.x, 0f, move.y);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Chimera"))
    //    {
    //        PH.Damage(enemyDamage);
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Chimera")
        {
            PH.Damage(enemyDamage);
        }
    }

    private void OnDestroy()
    {
        Dash.started -= Dash_started;
        Dash.canceled -= Dash_canceled;
        Shoot.started -= Shoot_started; 
        Shoot.canceled -= Shoot_canceled;
    }
}
