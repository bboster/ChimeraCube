using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Trinity
/// Description: Manages health for any object / creature that requires it
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    float maxHealth = 100;

    [Header("UI")]
    [SerializeField]
    Image fillImage;

    float currentHealth = 100;

    bool isFillImageNull = true;

    private void Start()
    {
        currentHealth = maxHealth;
        isFillImageNull = fillImage == null;
    }

    public void Damage(float dmg)
    {
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);

        if (!isFillImageNull)
            UpdateUI();
    }

    // UI
    private void UpdateUI()
    {
        fillImage.fillAmount = currentHealth / maxHealth;
    }

    // Getters & Setters
    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

}
