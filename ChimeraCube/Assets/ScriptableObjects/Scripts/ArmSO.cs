using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ArmSO : ScriptableObject
{
    public float maxHealth = 100;

    public float maxStamina = 100;

    public float staminaRegen = 5;

    public float staminaDecay = 0.1f;

    public float exhaustionMax = 10;

    public float exhaustionDecay = 1;

    public float maxStretchLength = 5;
}
