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

    [SerializeField] private Rigidbody rb;

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

        // Dash started and cancelled
        Dash.started += Dash_started;
        Dash.canceled += Dash_canceled;

        // Get the rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    private void Dash_canceled(InputAction.CallbackContext obj)
    {
        Debug.Log("Dash stopped");
    }

    private void Dash_started(InputAction.CallbackContext context)
    {
        Debug.Log("Dash started");
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
}
