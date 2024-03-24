using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ArmSO : ScriptableObject
{
    [Header("Stamina")]
    public float maxStamina = 100;

    [Header("Stretching")]
    public float maxStretchLength = 5;

    public float growthRate = 5;

    public float growthDecayRate = 0.1f;

    public bool doDecayOnHit = true;

    [Header("Environment")]
    public bool doDestroyEnvironment = true;

    [Header("Debug")]
    public KeyCode abilityTestKeycode = KeyCode.Space;

    public KeyCode damageTestKeycode = KeyCode.E;
}
