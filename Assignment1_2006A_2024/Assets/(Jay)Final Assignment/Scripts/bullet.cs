using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    public int bulletDamage;
    private void OnCollisionEnter(Collision hitObject)
    {
        if (hitObject.gameObject.CompareTag("Target"))
        {
            print("HIT" + hitObject.gameObject.name);
            bulletImpactEffect(hitObject);
            Destroy(gameObject);
        }

        if (hitObject.gameObject.CompareTag("Wall"))
        {
            print("Hit a wall");
            bulletImpactEffect(hitObject);
            Destroy(gameObject);
        }
        if (hitObject.gameObject.CompareTag("Enemy"))
        {

            if (hitObject.gameObject.GetComponent<Enemy>().isDead == false)
            {
               hitObject.gameObject.GetComponent<Enemy>().takeDamage(bulletDamage);
            }

            createBloodSprayEffect(hitObject);

            Destroy(gameObject);
        }
    }

    private void createBloodSprayEffect(Collision hitObject)
    {
        ContactPoint contact = hitObject.contacts[0];
        GameObject bloodSprayPrefab = Instantiate(
            globalReferences.Instance.bloodSprayEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );
        bloodSprayPrefab.transform.SetParent(hitObject.gameObject.transform);
    }

    void bulletImpactEffect(Collision hitObject)
    {
        ContactPoint contact = hitObject.contacts[0];
        GameObject hole = Instantiate(
            globalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );
        hole.transform.SetParent(hitObject.gameObject.transform);
    }

}
