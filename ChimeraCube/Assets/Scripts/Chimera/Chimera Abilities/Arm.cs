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
    ChimeraAbility attackAbil;

    [SerializeField]
    ChimeraAbility specialAbil;

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

    [Header("Abilities")]
    [SerializeField]
    float attackStartDelay = 1;

    [SerializeField]
    float specialStartDelay = 1;

    [SerializeField]
    float damage = 1;

    [SerializeField]
    float attackDamageMult = 5;

    // Public Assignments
    public Chimera Chimera { get; private set; }

    // Private Assignments
    Health health;

    public bool isAnimating = false;

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

        if(attackAbil != null)
            attackAbil.SetCurrentCooldown(attackAbil.GetCurrentCooldown() + attackStartDelay);

        if (specialAbil != null)
            specialAbil.SetCurrentCooldown(attackAbil.GetCurrentCooldown() + specialStartDelay);
    }

    // Stamina and Ability Tracking
    void Update()
    {
        if(Chimera.DebugToolsEnabled())
            DebugKeybinds();
    }

    private void DebugKeybinds()
    {
        if (attackAbil != null && Input.GetKeyUp(armData.attackTestCode))
            attackAbil.Execute();

        if (specialAbil != null && Input.GetKeyUp(armData.specialTestCode))
            specialAbil.Execute();

        if (Input.GetKeyUp(armData.damageTestKeycode))
            Damage(5.0f);
    }

    private void FixedUpdate()
    {
        Tick(Time.fixedDeltaTime);
    }

    private void Tick(float deltaTime)
    {
        AbilityTick();

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

            //Debug.Log(name + " is Decaying: " + timeScaledDecay);
            if (currentStamina <= 0)
                currentPendingDamage = 0;

            if (currentPendingDamage <= 0)
                this.staminaDecay = 0;
        }
        else if (!IsFullyStretched())
            currentStamina += deltaTime * armData.growthRate * growthSpeedModifier;
    }

    private void AbilityTick()
    {
        if (isAnimating)
            return;

        if(attackAbil != null && attackAbil.IsUsable())
            attackAbil.Execute();

        else if (specialAbil != null && specialAbil.IsUsable())
            specialAbil.Execute();
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

    public float GetGrowthDecayRate()
    {
        return armData.growthDecayRate;
    }

    public void SetStamina(float newStamina)
    {
        currentStamina = newStamina;
        if (currentStamina < 0)
            currentStamina = 0;
        else if(currentStamina > armData.maxStamina)
        {
            currentStamina = armData.maxStamina;
        }

        Stretch();
    }

    public float GetStamina()
    {
        return currentStamina;
    }

    public void DecayStamina(float staminaReduction)
    {
        currentPendingDamage += staminaReduction;

        staminaDecay = currentPendingDamage * armData.growthDecayRate;

        //Debug.Log(name + "'s Stamina Decay Is Set To: " + staminaDecay);
        if (currentStamina < 0)
            currentStamina = 0;
    }

    // Health and Damaging
    public void Damage(float dmg)
    {
        if(health == null)
        {
            Debug.LogError("Health Script Null!");
            return;
        }

        health.Damage(dmg);

        // Growth Decay
        if (armData.doDecayOnHit)
        {
            DecayStamina(dmg * armData.decayRateMultiplier);
        }
        
        if (health.IsDead())
        {
            Debug.Log(gameObject + " died!");
            gameObject.SetActive(false);
            Chimera.IncrementRotationSpeed();
            return;
        }

        //Debug.Log("-=-=-=-=-=-=-=-=-=-=-=-=-=-");
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log(name + " damaged player!");
            float damage = isAnimating ? this.damage * attackDamageMult : this.damage;
            collision.collider.GetComponent<PlayerHealth>().Damage(damage);
        }
    }*/

    public float GetAttackDamage()
    {
        return isAnimating ? this.damage * attackDamageMult : this.damage;
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

    public void SetIsAnimating(bool isAnim)
    {
        Debug.Log(name + " IS ANIMATING: " + isAnim);
        isAnimating = isAnim;
    }

    public bool IsAnimating()
    {
        return isAnimating;
    }
}
