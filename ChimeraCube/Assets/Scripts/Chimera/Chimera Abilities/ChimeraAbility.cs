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

    private void Start()
    {
        arm = GetComponentInParent<Arm>();

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

    // Ability Usage
    public void Execute()
    {
        if (!IsUsable())
            return;

        ChildExecute();

        currentCooldown = abilData.cooldown;
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
