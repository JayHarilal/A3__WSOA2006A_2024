using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwables : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;

    float countdown;

    bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum ThrowableType
    {
        None,
        Grenade,
        Flash
    }

    public ThrowableType throwabletype;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if(countdown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        getThrowableEffect();

        Destroy(gameObject);
    }

    private void getThrowableEffect()
    {
        switch (throwabletype)
        {
            case ThrowableType.Grenade:
                grenadeEffect();
                break;
            case ThrowableType.Flash:
                flashEffect();
                break;
        }
    }

    private void flashEffect()
    {
        GameObject flashEffect = globalReferences.Instance.flashGrenadeEffect;
        Instantiate(flashEffect, transform.position, transform.rotation);

        soundManager.Instance.throwablesChannel.PlayOneShot(soundManager.Instance.grenadeSound);


        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //apply blindness
            }
        }
    }

    private void grenadeEffect()
    {
        GameObject explosionEffect = globalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        soundManager.Instance.throwablesChannel.PlayOneShot(soundManager.Instance.grenadeSound);


        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb!=null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            if (objectInRange.gameObject.GetComponent<Enemy>())
            {
                objectInRange.gameObject.GetComponent<Enemy>().takeDamage(100);
            }
        }
    }
}
