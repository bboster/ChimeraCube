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

    [SerializeField]
    LayerMask playerLayer;

    [SerializeField]
    LayerMask environmentLayer;

    [Header("UI Assignments")]
    [SerializeField]
    Image fillImage1;

    [SerializeField]
    Image fillImage2;

    [Header("Stats")]
    [SerializeField]
    float armSpreadAngle = 45;

    [SerializeField]
    float internalCooldown = 5;

    [SerializeField]
    float internalCooldownVariance = 3;

    [SerializeField]
    float waitBeforeSecondPlayerCheck = 0.5f;

    [SerializeField]
    float duration = 10;

    [SerializeField]
    float staminaBonus = 2.5f;

    Health subArmHealth1, subArmHealth2;

    Coroutine durationCoroutine;

    Collider[] inRangeColliders = new Collider[1000];

    protected override void ChildStart()
    {
        subArmHealth1 = subArm1.GetComponent<Health>();
        subArmHealth2 = subArm2.GetComponent<Health>();
    }

    protected override void ChildExecute()
    {
        //Debug.Log("Attempting Special!");
        bool playerCheck = PlayerCheck();
        bool environmentCheck = EnvironmentCheck();

        //Debug.Log("Player Check: " + playerCheck);

        subArm1.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, armSpreadAngle / 2, 0));
        subArm2.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles - new Vector3(0, armSpreadAngle / 2, 0));

        //Debug.Log("Environment Check:" + environmentCheck);
        if (!playerCheck || !environmentCheck)
        {
            SetCurrentCooldown(internalCooldown + Random.Range(-internalCooldownVariance, internalCooldownVariance));
            //Debug.Log("Checks Went Wrong! Triggering ICD!");
            DelayedSetAnimating(false, 0.1f);
            //Debug.Log("My arm is: " + arm.name);

            return;
        }

        SubscribeEvents();

        //Debug.Log("Special Execution");
        arm.SetPauseGrowth(true);
        arm.SetArmTransformActive(false);
        arm.SetHandTransformActive(false);

        subArm1.SetStamina(arm.GetStamina());

        subArm2.SetGrowthSpeedModifier(2);

        EnableArm(subArm1);
        EnableArm(subArm2);

        durationCoroutine = StartCoroutine(DurationElapse());
    }

    private IEnumerator DurationElapse()
    {
        yield return new WaitForSeconds(duration);
        StartCoroutine(StopAttack());
    }

    private IEnumerator StopAttack()
    {
        subArm1.DecayStamina(subArm1.GetStamina());
        subArm2.DecayStamina(subArm2.GetStamina());
        yield return new WaitForSeconds(subArm1.GetStamina() * subArm1.GetGrowthDecayRate());
        arm.SetStamina(arm.GetStamina() + staminaBonus);
        OnArmDeath();
    }

    private IEnumerator WaitForPlayerCheck()
    {
        yield return new WaitForSeconds(waitBeforeSecondPlayerCheck);
        if (!PlayerCheck())
            StartCoroutine(StopAttack());
    }

    bool PlayerCheck()
    {
        int inRangeColliderCount = Physics.OverlapSphereNonAlloc(transform.position, 1000, inRangeColliders, playerLayer);

        Vector3 characterToCollider;
        float dot;
        for (int i = 0; i < inRangeColliderCount; i++)
        {
            characterToCollider = (inRangeColliders[i].transform.position - transform.position).normalized;
            dot = Vector3.Angle(characterToCollider, transform.TransformDirection(Vector3.forward).normalized);
            if (dot < armSpreadAngle / 2)
            {
                return true;
            }
        }

        return false;
    }

    bool EnvironmentCheck()
    {
        return Physics.Raycast(subArm1.transform.position, subArm1.transform.TransformDirection(Vector3.forward) * 1000, environmentLayer) &&
            Physics.Raycast(subArm2.transform.position, subArm2.transform.TransformDirection(Vector3.forward) * 1000, environmentLayer);
    }

    private void OnArmDeath()
    {
        //Debug.Log("Arm Death");
        DisableArm(subArm1, subArmHealth1);
        DisableArm(subArm2, subArmHealth2);
        
        arm.SetPauseGrowth(false);
        arm.SetArmTransformActive(true);
        arm.SetHandTransformActive(true);

        arm.SetIsAnimating(false);

        arm.Chimera.ToggleRotation(true);

        if(durationCoroutine != null)
        {
            StopCoroutine(durationCoroutine);
        }

        durationCoroutine = null;

        SetCurrentCooldown(internalCooldown + Random.Range(-internalCooldownVariance, internalCooldownVariance));

        UnsubscribeEvents();
    }

    private void OnArmFullyStretched()
    {
        //Debug.Log("Stretcheddd");
        arm.Chimera.ToggleRotation(false);

        StartCoroutine(WaitForPlayerCheck());

        subArm2.FinishedGrowthEvent -= OnArmFullyStretched;
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

    private void SubscribeEvents()
    {
        subArmHealth1.DeathEvent += OnArmDeath;
        subArmHealth2.DeathEvent += OnArmDeath;
        subArm2.FinishedGrowthEvent += OnArmFullyStretched;
    }

    private void UnsubscribeEvents()
    {
        subArmHealth1.DeathEvent -= OnArmDeath;
        subArmHealth2.DeathEvent -= OnArmDeath;
        subArm2.FinishedGrowthEvent -= OnArmFullyStretched;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(subArm1.transform.position, subArm1.transform.TransformDirection(Vector3.forward) * 1000);
        Gizmos.DrawLine(subArm2.transform.position, subArm2.transform.TransformDirection(Vector3.forward) * 1000);
    }*/
}
