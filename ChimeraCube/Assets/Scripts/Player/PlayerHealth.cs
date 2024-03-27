using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //public static PlayerHealth instance;

    [SerializeField] private float maxHealth = 300f;
    [SerializeField] private float playerHealth;
    [SerializeField] private Collider hitBox;
    [SerializeField] private bool isHit;
    [SerializeField] private float iFrameDuration;

    [SerializeField] private GameObject youDiedCanvas;

    [SerializeField] Image healthBar; 

    

    // Start is called before the first frame update
    void Start()
    {
       // instance = this;
        playerHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth < 0f)
        {
            playerHealth = 0f;
            gameObject.SetActive(false);
            youDiedCanvas.SetActive(true);
        }
        else if(playerHealth > maxHealth)
        {
            playerHealth = maxHealth;
        }

        //HealthFill.fillAmount = Mathf.Lerp(HealthFill.fillAmount, (playerHealth / maxHealth), 0.5f);
    }
    public void Damage(float damage)
    {
        if (isHit)
        {
            return;
        }
        playerHealth -= damage;
        isHit = true;
        StartCoroutine(IFrames());
        //if (playerHealth <= 0)
        //{
        //    ScoreboardManager.Instance.StopGame();
        //}
        //Debug.Log("Hit!");
        UpdateUI();
    }

    public void Heal(float amt)
    {
        float temp = playerHealth + amt;
        if (temp > maxHealth)
            temp = maxHealth;

        playerHealth = temp;
        UpdateUI();
    }

    IEnumerator IFrames()
    {
        yield return new WaitForSeconds(iFrameDuration);
        isHit = false;
    }

    private void UpdateUI()
    {
        healthBar.fillAmount = playerHealth / maxHealth;
    }
}
