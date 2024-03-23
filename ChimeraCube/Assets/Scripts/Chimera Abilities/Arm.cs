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

    // Private Assignments
    Health health;

    // Stamina Management
    float currentStamina = 0;

    float currentPendingDamage = 0;

    float staminaDecay = 0;

    private void Start()
    {
        health = GetComponent<Health>();

        currentStamina = 0;
        currentPendingDamage = 0;
    }

    // Stamina and Ability Tracking
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            abil.Execute();

        if (Input.GetKeyUp(KeyCode.E))
            Damage(10.0f);
    }

    private void FixedUpdate()
    {
        Tick(Time.fixedDeltaTime);
    }

    private void Tick(float deltaTime)
    {
        StaminaTick(deltaTime);

        Stretch();
    }

    private void StaminaTick(float deltaTime)
    {
        if (IsExhausted())
        {
            float timeScaledDecay = this.staminaDecay * deltaTime;
            currentPendingDamage -= timeScaledDecay;

            currentPendingDamage -= timeScaledDecay;
            currentStamina -= timeScaledDecay;

            if (currentPendingDamage <= 0)
                this.staminaDecay = 0;
        }
        else if (!IsFullyStretched())
            currentStamina += deltaTime * armData.growthRate;
    }

    public bool IsFullyStretched()
    {
        return currentStamina >= armData.maxStamina;
    }

    public bool IsExhausted()
    {
        return currentPendingDamage > 0;
    }

    // Stretching
    private void Stretch()
    {
        float newScale = Mathf.Clamp01(currentStamina / armData.maxStamina) * armData.maxStretchLength;

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
        health.Damage(dmg);
        currentStamina -= currentPendingDamage;
        Debug.Log("Cleared Pending: " + currentPendingDamage);

        currentPendingDamage = dmg;
        Debug.Log("New Pending Damage: " + currentPendingDamage);

        staminaDecay = currentPendingDamage * armData.growthDecayRate;

        if (currentStamina < 0)
            currentStamina = 0;

        if (health.IsDead())
        {
            Debug.Log(gameObject + " died!");
            return;
        }

        Debug.Log("-=-=-=-=-=-=-=-=-=-=-=-=-=-");
    }
}
