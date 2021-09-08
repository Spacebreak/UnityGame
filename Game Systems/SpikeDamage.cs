using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public HealthManager luperHealthManager;    //Script managing Lupers health.
    public LuperController luperCharacter;      //Script managing Lupers movement.

    public float xKnockback;                    //Knockback force to be applied along x-axis.
    public float yKnockback;                    //Knockback force to be applied along y-axis.

    private readonly string playerString = "Player"; //The string holding reference to GameObject tag.

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerString))
        {
            luperHealthManager.LuperDamaged();
            luperCharacter.LuperKnockback(transform.position, xKnockback, yKnockback);
        }
    }
}
