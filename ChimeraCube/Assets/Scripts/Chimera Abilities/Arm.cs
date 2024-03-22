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

    float currentExhaustion = 0;

    float pendingDamage = 0;

    private void Start()
    {
        currentStamina = 0;
        currentExhaustion = 0;
        pendingDamage = 0;

        currentHealth = armData.maxHealth;
    }

    // Stamina and Ability Tracking
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            abil.Execute();

        if (Input.GetKeyUp(KeyCode.E))
            Damage(5.0f);
    }

    private void FixedUpdate()
    {
        Tick(Time.fixedDeltaTime);
    }

    private void Tick(float deltaTime)
    {
        if (!IsFullyStretched() && !IsExhausted())
            currentStamina += deltaTime * armData.staminaRegen;

        Stretch(deltaTime);
    }

    public bool IsFullyStretched()
    {
        return currentStamina >= armData.maxStamina;
    }

    public bool IsExhausted()
    {
        //return currentExhaustion > 0;
        return pendingDamage > 0;
    }

    // Stretching
    private void Stretch(float deltaTime)
    {
        
        float staminaDecay = pendingDamage * armData.staminaDecay * deltaTime;
        pendingDamage -= staminaDecay;

        float newScale = ((currentStamina - staminaDecay) / armData.maxStamina) * armData.maxStretchLength;

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
        currentStamina -= pendingDamage;
        Debug.Log("Cleared Pending: " + pendingDamage);

        pendingDamage = dmg;
        Debug.Log("New Pending Damage: " + pendingDamage);

        Debug.Log(gameObject + " ouch!");

        if (currentStamina < 0)
            currentStamina = 0;

        if (currentHealth <= 0)
        {
            Debug.Log(gameObject + " died!");
            return;
        }
    }
}
