using System.Collections;
using UnityEngine;

public class EnemyInteraction : MonoBehaviour
{
    private LuperController luperCharacter;      //Script managing Lupers movement.

    public float xKnockback;                    //Knockback force to be applied along x-axis.
    public float yKnockback;                    //Knockback force to be applied along y-axis.

    private bool stunCooldown;                  //How often the enemy can stun the player.

    public AudioSource deathSound;              //The sound played on death.
    public GameObject deathParticlesObject;     //The object that holds the death particles. 
    public ParticleSystem deathParticles;       //The particles that emit on death.

    private readonly string playerString = "Player"; //The string holding reference to GameObject tag.
    private HealthManager luperHealthManager;    //Script managing Lupers health.

    private void Start()
    {
        luperCharacter = FindObjectOfType<LuperController>();
        luperHealthManager = FindObjectOfType<HealthManager>();

        if(luperCharacter == null)
        {
            Debug.Log("EnemyInteraction: Couldn't find LuperController class.");
        }

        if (luperHealthManager == null)
        {
            Debug.Log("EnemyInteraction: Couldn't find LuperHealthManager class.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerString))
        {
            if (luperCharacter.GetLuperSpin())
            {
                Death(collision);
                return;
            }

            if (!stunCooldown)
            {
                HurtPlayer();
                StartCoroutine(EnemyStunned());
            }

            //ParticleSystem.VelocityOverLifetimeModule particleVelocity = ParticleSystem.VelocityOverLifetimeModule;
            //var particleDirection = ParticleSystem.VelocityOverLifetimeModule;

            //deathParticles.velocityOverLifetime.x = playerDirection.x;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerString))
        {
            if (luperCharacter.GetLuperSpin())
            {
                Death(collision);
                return;
            }

            if (!stunCooldown)
            {
                HurtPlayer();
                StartCoroutine(EnemyStunned());
            }

            //ParticleSystem.VelocityOverLifetimeModule particleVelocity = ParticleSystem.VelocityOverLifetimeModule;
            //var particleDirection = ParticleSystem.VelocityOverLifetimeModule;

            //deathParticles.velocityOverLifetime.x = playerDirection.x;
        }
    }

    private void HurtPlayer()
    {
        luperHealthManager.LuperDamaged();
        luperCharacter.LuperKnockback(transform.position, xKnockback, yKnockback);
    }

    private IEnumerator EnemyStunned()
    {
        stunCooldown = true;
        yield return new WaitForSeconds(1.4f);
        stunCooldown = false;
    }

    private void Death(Collision2D collision)
    {
        deathSound.Play();
        DeathParticles(collision);
        Destroy(gameObject);
    }

    //Sets the position of the death particles and the direction.
    private void DeathParticles(Collision2D collision)
    {
        deathParticlesObject.transform.position = collision.rigidbody.position;

        Vector2 playerDirection = transform.position - collision.transform.position;

        deathParticles.Play();

        var particle = deathParticles.velocityOverLifetime;
        particle.x = playerDirection.x * 10;
        particle.y = playerDirection.y * 6;
    }
}
