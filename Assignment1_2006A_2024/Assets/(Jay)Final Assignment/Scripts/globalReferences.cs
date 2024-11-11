using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalReferences : MonoBehaviour
{
    public static globalReferences Instance { get; set; }

    public GameObject bulletImpactEffectPrefab;

    public GameObject grenadeExplosionEffect;
    public GameObject flashGrenadeEffect;

    public GameObject bloodSprayEffect;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
