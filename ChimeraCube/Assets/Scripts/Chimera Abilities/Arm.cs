using System;
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

    [Header("Visual Assignments")]
    [SerializeField]
    Renderer handRenderer;

    [SerializeField]
    Renderer armRenderer;

    [Header("Growth")]
    [SerializeField]
    bool pauseGrowth = false;

    [SerializeField]
    float growthSpeedModifier = 1;

    // Public Assignments
    public Chimera Chimera { get; private set; }

    // Private Assignments
    Health health;

    // Stamina Management
    float currentStamina = 0;

    float currentPendingDamage = 0;

    float staminaDecay = 0;

    // Events
    public event Action FinishedGrowthEvent;

    private void Start()
    {
        health = GetComponent<Health>();
        Chimera = GetComponentInParent<Chimera>();
    }

    // Stamina and Ability Tracking
    void Update()
    {
        if (abil != null && Input.GetKeyUp(armData.abilityTestKeycode))
            abil.Execute();

        if (Input.GetKeyUp(armData.damageTestKeycode))
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
            currentStamina += deltaTime * armData.growthRate * growthSpeedModifier;
    }

    public bool IsFullyStretched()
    {
        bool isFullyStretched = currentStamina >= armData.maxStamina;
        if (isFullyStretched)
            FinishedGrowthEvent?.Invoke();

        return isFullyStretched;
    }

    public bool IsExhausted()
    {
        return currentPendingDamage > 0 || pauseGrowth;
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

    public void SetPauseGrowth(bool doPause)
    {
        pauseGrowth = doPause;
    }

    public void SetGrowthSpeedModifier(float newGrowthSpeedMod)
    {
        growthSpeedModifier = newGrowthSpeedMod;
    }

    public float GetGrowthSpeedModifier()
    {
        return growthSpeedModifier;
    }

    public void SetStamina(float newStamina)
    {
        currentStamina = newStamina;
        Stretch();
    }

    public float GetStamina()
    {
        return currentStamina;
    }

    // Health and Damaging
    public void Damage(float dmg)
    {
        health.Damage(dmg);

        // Growth Decay
        if (armData.doDecayOnHit)
        {
            currentStamina -= currentPendingDamage;
            //Debug.Log("Cleared Pending: " + currentPendingDamage);

            currentPendingDamage = dmg;
            //Debug.Log("New Pending Damage: " + currentPendingDamage);

            staminaDecay = currentPendingDamage * armData.growthDecayRate;

            if (currentStamina < 0)
                currentStamina = 0;
        }
        
        if (health.IsDead())
        {
            Debug.Log(gameObject + " died!");
            return;
        }

        //Debug.Log("-=-=-=-=-=-=-=-=-=-=-=-=-=-");
    }

    // Arm Segments
    public bool GetArmTransformActive()
    {
        return armTransform.gameObject.activeSelf;
    }

    public void SetArmTransformActive(bool active)
    {
        armTransform.gameObject.SetActive(active);
    }

    public bool GetHandTransformActive()
    {
        return handTransform.gameObject.activeSelf;
    }

    public void SetHandTransformActive(bool active)
    {
        handTransform.gameObject.SetActive(active);
    }
}
