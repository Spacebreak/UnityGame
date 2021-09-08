using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowCoinParticleEffect : MonoBehaviour
{
    public TokenYellow yellowTokenScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            yellowTokenScript.EnableParticleSystem(transform.position);
        }
    }
}
