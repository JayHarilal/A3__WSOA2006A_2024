using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionManager : MonoBehaviour
{
    public static interactionManager Instance { get; set; }

    public Weapons hoveredOverWeapon = null;
    public Throwables hoveredOverThrowable = null;
    public ammoBox hoveredOverAmmoBox = null;
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

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            if (objectHitByRaycast.GetComponent<Weapons>() && objectHitByRaycast.GetComponent<Weapons>().isActiveWeapon == false)
            {
                if (hoveredOverWeapon)
                {
                    hoveredOverWeapon.GetComponent<Outline>().enabled = false;
                }

                hoveredOverWeapon = objectHitByRaycast.gameObject.GetComponent<Weapons>();
                hoveredOverWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    weaponManager.Instance.pickUpWeapon(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredOverWeapon)
                {
                    hoveredOverWeapon.GetComponent<Outline>().enabled = false;
                }
            }
            //ammobox
            if (objectHitByRaycast.GetComponent<ammoBox>())
            {
                if (hoveredOverAmmoBox)
                {
                    hoveredOverAmmoBox.GetComponent<Outline>().enabled = false;
                }
                hoveredOverAmmoBox = objectHitByRaycast.gameObject.GetComponent<ammoBox>();
                hoveredOverAmmoBox.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    weaponManager.Instance.pickUpAmmo(hoveredOverAmmoBox);
                    Destroy(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredOverAmmoBox)
                {
                    hoveredOverAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }
            //throwable
            if (objectHitByRaycast.GetComponent<Throwables>())
            {
                if (hoveredOverThrowable)
                {
                    hoveredOverThrowable.GetComponent<Outline>().enabled = false;
                }
                hoveredOverThrowable = objectHitByRaycast.gameObject.GetComponent<Throwables>();
                hoveredOverThrowable.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    weaponManager.Instance.pickUpThrowable(hoveredOverThrowable);
                }
            }
            else
            {
                if (hoveredOverThrowable)
                {
                    hoveredOverThrowable.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
