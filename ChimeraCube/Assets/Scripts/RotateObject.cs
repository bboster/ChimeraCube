using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateObject : MonoBehaviour
{
    [SerializeField]
    Vector3 rotation;

    [SerializeField]
    float rotationRate = 1;

    void FixedUpdate()
    {
        Rotate(Time.fixedDeltaTime);
    }

    private void Rotate(float deltaTime)
    {
        Vector3 newRotation = transform.rotation.eulerAngles + rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), rotationRate * deltaTime);
    }
}
