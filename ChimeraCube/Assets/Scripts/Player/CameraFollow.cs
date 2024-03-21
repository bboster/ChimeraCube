// Script written by: Brooke Boster

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f; // How fast the camera follows the player
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        // As long as we have a target, add the offset to that position and then follow the player!
        if(target != null)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
