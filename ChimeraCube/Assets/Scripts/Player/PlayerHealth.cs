using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //public static PlayerHealth instance;

    [SerializeField] private float maxHealth = 300f;
    [SerializeField] private float playerHealth;
    [SerializeField] private Collider hitBox;
    [SerializeField] private bool isHit;
    [SerializeField] private float iFrameDuration;

    [SerializeField] private GameObject youDiedCanvas;

    

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
    }

    public void Heal(float amt)
    {
        float temp = playerHealth + amt;
        if (temp > maxHealth)
            temp = maxHealth;

        playerHealth = temp;
    }

    IEnumerator IFrames()
    {
        yield return new WaitForSeconds(iFrameDuration);
        isHit = false;
    }
}
