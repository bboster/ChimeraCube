// Script written by: Brooke Boster

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // The speed of the player and the move input!
    [SerializeField] private float speed;
    private Vector2 move;

    // Gets the movement input from the InputActions action map.
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
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
