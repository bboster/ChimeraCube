using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Trinity
/// Description: Controls Arm behavior
/// </summary>
public class Arm : MonoBehaviour
{
    [SerializeField]
    ArmSO armData;

    [Header("Logic Assignments")]
    [SerializeField]
    ChimeraAbility abil;

    [SerializeField]
    Transform handTransform;

    [SerializeField]
    Transform armTransform;

    [SerializeField]
    Collider handCollider;

    [SerializeField]
    Collider armCollider;

    [Header("Visual Assignments")]
    [SerializeField]
    Renderer handRenderer;

    [SerializeField]
    Renderer armRenderer;

    float currentHealth = 100;

    float currentStamina = 0;

    private void Start()
    {
        currentStamina = 0;
        currentHealth = armData.maxHealth;
    }

    // Stamina and Ability Tracking
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            abil.Execute();
    }

    private void FixedUpdate()
    {
        Tick(Time.fixedDeltaTime);
    }

    private void Tick(float deltaTime)
    {
        if (!IsFullyStretched())
            currentStamina += deltaTime * armData.staminaRegen;

        Stretch();
    }

    public bool IsFullyStretched()
    {
        return currentStamina >= armData.maxStamina;
    }

    // Stretching
    private void Stretch()
    {
        float newScale = (currentStamina / armData.maxStamina) * armData.maxStretchLength;

        Vector3 tempScale = armTransform.localScale;
        tempScale.z = newScale;
        armTransform.localScale = tempScale;

        tempScale = handTransform.localPosition;
        tempScale.z = newScale * 2;
        handTransform.localPosition = tempScale;
    }

    // Health and Damaging
    public void Damage(float dmg)
    {
        currentHealth -= dmg;
        Debug.Log(gameObject + " ouch!");

        if (currentHealth <= 0)
        {
            Debug.Log(gameObject + " died!");
            return;
        }
    }
}
