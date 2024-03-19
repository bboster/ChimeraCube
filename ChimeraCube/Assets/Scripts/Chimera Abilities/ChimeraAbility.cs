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

    float currentCooldown = 0;

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

    protected abstract void ChildExecute();
}
