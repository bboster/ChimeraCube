using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : ChimeraAbility
{
    Animator armAnim;

    protected override void ChildStart()
    {
        armAnim = arm.GetComponent<Animator>();
    }

    protected override void ChildExecute()
    {
        if (armAnim == null)
        {
            Debug.LogError("ArmAnim is null!");
            return;
        }

        armAnim.Play("FullSwipe");
    }
}
