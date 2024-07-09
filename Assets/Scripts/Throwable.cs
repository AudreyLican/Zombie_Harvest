using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;

    float countdown;

    bool hasExploded = false;
    public bool hasBeenThrown = false;
    public bool pickedUp = false;

    public enum ThrowableType
    {
        None,
        Grenade,
        Smoke_Grenade
    }

    public ThrowableType throwableType;

    private Rigidbody rb;

    private void Start()
    {
        countdown = delay;

        // Enssure the grenade starts as kinematic with gravity disabled to keep it in the air
        Rigidbody rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true; //set hasExploded to true so if statement to be false
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect();

        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
            case ThrowableType.Smoke_Grenade:
                SmokeGrenadeEffect();
                break;
        }
    }

    /**
     * Physical Effect : 
     * Create a sphere (= to damageRadius) to detecte all the collider inside
     * The collider detected inside the sphere are stored in 'colliders' array. We loop inside to inflect damage/effect to them
     * The explosion force will push enemies
     */
    private void SmokeGrenadeEffect()
    {
        // Visual Effect
        GameObject smokeEffect = GlobalReferences.Instance.smokeGrenadeEffect;
        Instantiate(smokeEffect, transform.position, transform.rotation); //Instantiate visual effect of explosion

        //Play Sound
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.smokeGrenadeSound);

        // Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Aplpy blindess to enemies
            }
        }
    }
    
    private void GrenadeEffect()
    {
        // Visual Effect
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation); //Instantiate visual effect of explosion

        //Play Sound
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeSound);

        // Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            // Damge over enemies
            if (objectInRange.gameObject.GetComponent<Enemy>())
            {
                objectInRange.gameObject.GetComponent<Enemy>().TakeDamage(100);
            }
        }

        
    }
}

