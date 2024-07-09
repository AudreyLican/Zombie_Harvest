using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; set; }

    public GameObject bulletImpactEffectPrefab;

    public GameObject grenadeExplosionEffect;
    public GameObject smokeGrenadeEffect;

    public GameObject bloodSprayEffect;

    public int waveNumber;

    //Check if instance is not null
    // This is a singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
