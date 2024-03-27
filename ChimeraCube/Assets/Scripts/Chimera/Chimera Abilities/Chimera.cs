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

    private void Start()
    {
        rotationalScript = GetComponent<RotateObject>();
    }

    public void SetRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
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
        SetRotationSpeed(rotationSpeedIncrement);
    }

    public bool DebugToolsEnabled()
    {
        return debugToolsEnabled;
    }
}
