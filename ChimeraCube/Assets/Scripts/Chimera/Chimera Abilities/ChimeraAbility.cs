using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Trinity
/// Description: Abstract class to be used to create abilities off of
/// </summary>
public abstract class ChimeraAbility : MonoBehaviour
{
    [SerializeField]
    ChimeraAbilitySO abilData;

    protected Arm arm { get; private set; }

    float currentCooldown = 0;

    private void Awake()
    {
        SetCooldown();
        arm = GetComponentInParent<Arm>();
    }

    private void Start()
    {
        ChildStart();
    }

    // Cooldown Functions
    private void FixedUpdate()
    {
        Tick(Time.fixedDeltaTime);
    }

    private void Tick(float deltaTime)
    {
        if (currentCooldown <= 0)
            return;

        currentCooldown -= deltaTime;

        ChildTick();
    }

    public bool IsUsable()
    {
        return currentCooldown <= 0;
    }

    private void SetCooldown()
    {
        currentCooldown = abilData.cooldown + Random.Range(-abilData.cooldownVariance, abilData.cooldownVariance);
    }

    // Ability Usage
    public void Execute()
    {
        if (!IsUsable())
            return;

        ChildExecute();

        SetCooldown();

        arm.SetIsAnimating(true);
    }

    // Getters and Setters
    public float GetCurrentCooldown()
    {
        return currentCooldown;
    }

    public void SetCurrentCooldown(float newCurrentCooldown)
    {
        currentCooldown = newCurrentCooldown;
    }

    // Child Functions
    protected abstract void ChildExecute();

    protected virtual void ChildStart()
    {

    }

    protected virtual void ChildTick()
    {

    }
}
