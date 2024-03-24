using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClawSpecial : ChimeraAbility
{
    [Header("Logic Assignments")]
    [SerializeField]
    Transform subArm1;

    [SerializeField]
    Transform subArm2;

    [Header("UI Assignments")]
    [SerializeField]
    Image fillImage1;

    [SerializeField]
    Image fillImage2;


    protected override void ChildExecute()
    {
        
    }
}
