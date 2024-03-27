using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimera : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField]
    RotateObject rotationalScript;

    [SerializeField]
    float rotationSpeed = 45;

    [SerializeField]
    float rotationSpeedIncrement = 1;

    [SerializeField]
    bool debugToolsEnabled = false;

    int amtStoppingRotation = 0;

    private void Start()
    {
        rotationalScript = GetComponent<RotateObject>();

        SetRotationSpeed(rotationSpeed);
    }

    public void SetRotationSpeed(float newSpeed)
    {
        float rotationSpeed = newSpeed * (CanRotate() ? 1 : 0);
        rotationalScript.SetRotationRate(rotationSpeed);
    }

    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

    public float GetRotationSpeedIncrement()
    {
        return rotationSpeedIncrement;
    }

    public void IncrementRotationSpeed()
    {
        rotationSpeed += rotationSpeedIncrement;
        SetRotationSpeed(rotationSpeed);
    }

    public bool DebugToolsEnabled()
    {
        return debugToolsEnabled;
    }

    public void ToggleRotation(bool toggle)
    {
        //Debug.Log("Toggled: " + toggle);
        amtStoppingRotation += toggle ? -1 : 1;
        if (amtStoppingRotation < 0)
            amtStoppingRotation = 0;
        SetRotationSpeed(rotationSpeed);
    }

    public bool CanRotate()
    {
        return amtStoppingRotation <= 0;
    }
}
