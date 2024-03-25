using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClawSpecial : ChimeraAbility
{
    [Header("Logic Assignments")]
    [SerializeField]
    Arm subArm1;

    [SerializeField]
    Arm subArm2;

    [Header("UI Assignments")]
    [SerializeField]
    Image fillImage1;

    [SerializeField]
    Image fillImage2;

    [Header("Stats")]
    [SerializeField]
    float armSpreadAngle = 45;

    Health subArmHealth1, subArmHealth2;

    protected override void ChildStart()
    {
        subArmHealth1 = subArm1.GetComponent<Health>();
        subArmHealth1.DeathEvent += OnArmDeath;

        subArmHealth2 = subArm2.GetComponent<Health>();
        subArmHealth2.DeathEvent += OnArmDeath;

        subArm2.FinishedGrowthEvent += OnArmFullyStretched;
    }

    protected override void ChildExecute()
    {
        arm.SetPauseGrowth(true);
        arm.SetArmTransformActive(false);
        arm.SetHandTransformActive(false);

        subArm1.SetStamina(arm.GetStamina());

        subArm2.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, armSpreadAngle, 0));
        subArm2.SetGrowthSpeedModifier(2);

        EnableArm(subArm1);
        EnableArm(subArm2);
    }

    private void OnArmDeath()
    {
        DisableArm(subArm1, subArmHealth1);
        DisableArm(subArm2, subArmHealth2);

        arm.Chimera.SetRotationSpeed(1);
        arm.SetPauseGrowth(false);
        arm.SetArmTransformActive(true);
        arm.SetHandTransformActive(true);
    }

    private void OnArmFullyStretched()
    {
        arm.Chimera.SetRotationSpeed(0);
    }

    private void EnableArm(Arm arm)
    {
        arm.gameObject.SetActive(true);

        arm.SetPauseGrowth(false);
    }

    private void DisableArm(Arm arm, Health health)
    {
        arm.gameObject.SetActive(false);

        arm.SetStamina(0);
        arm.SetGrowthSpeedModifier(1);
        health.SetHealth(health.GetMaxHealth());

        arm.SetPauseGrowth(true);
    }
}
