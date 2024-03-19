using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAbility : ChimeraAbility
{
    [SerializeField]
    ParticleSystem particles;

    protected override void ChildExecute()
    {
        Debug.Log("Ability Executed!");
        if (particles != null)
            particles.Play();
    }
}
