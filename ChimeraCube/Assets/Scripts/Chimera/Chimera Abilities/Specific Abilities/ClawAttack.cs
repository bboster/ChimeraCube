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
            arm.SetIsAnimating(false);
            return;
        }

        armAnim.Play("FullSwipe");
        StartCoroutine(DelayedClearAnimating());
    }

    private IEnumerator DelayedClearAnimating()
    {
        //Debug.Log("Anim will clear in: " + 1 / armAnim.speed);
        yield return new WaitForSeconds(1 / armAnim.speed);
        //Debug.Log("CLEARED!");
        arm.SetIsAnimating(false);
    }
}
