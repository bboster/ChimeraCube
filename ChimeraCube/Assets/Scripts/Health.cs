using System;
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

    // Events
    public event Action DamageTakenEvent;

    public event Action DeathEvent;

    private void Start()
    {
        currentHealth = maxHealth;
        isFillImageNull = fillImage == null;
    }

    public void Damage(float dmg)
    {
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);

        DamageTakenEvent?.Invoke();
        //Debug.Log(gameObject + " damaged!");

        UpdateUI();

        if (IsDead())
        {
            DeathEvent?.Invoke();

            gameObject.SetActive(false);
        }
            
    }

    // UI
    private void UpdateUI()
    {
        if (isFillImageNull)
            return;

        fillImage.fillAmount = currentHealth / maxHealth;
    }

    public void SetFillImage(Image image)
    {
        fillImage = image;
        UpdateUI();
    }

    public Image GetFillImage()
    {
        return fillImage;
    }

    // Getters & Setters
    public float GetHealth()
    {
        return currentHealth;
    }

    public void SetHealth(float newHealth)
    {
        currentHealth = newHealth;
        UpdateUI();
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
