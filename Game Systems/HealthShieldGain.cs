using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthShieldGain : MonoBehaviour
{
    private readonly string playerString = "Player"; //The string holding reference to GameObject tag.

    public bool isHealth;
    public bool isShield;

    private HealthManager playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(playerHealth == null)
        {
            playerHealth = collision.GetComponentInChildren<HealthManager>();
        }

        if (playerHealth != null && collision.CompareTag(playerString))
        {
            if (isHealth && playerHealth.GetHealth() != 3)
            {
                playerHealth.GainHealth();
                Destroy(gameObject);
            }
            else if (isShield && playerHealth.GetShield() != 3)
            {
                playerHealth.GainShield();
                Destroy(gameObject);
            }
        }
    }

    
}
