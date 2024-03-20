using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAbility : ChimeraAbility
{
    [SerializeField]
    ParticleSystem particles;

    Animator armAnim;

    protected override void ChildStart()
    {
        armAnim = arm.GetComponent<Animator>();
    }

    protected override void ChildExecute()
    {
        Debug.Log("Ability Executed!");
        if (particles != null)
            particles.Play();

        if(armAnim == null)
        {
            Debug.LogError("ArmAnim is null!");
            return;
        }

        armAnim.Play("Swivel");
    }
}
