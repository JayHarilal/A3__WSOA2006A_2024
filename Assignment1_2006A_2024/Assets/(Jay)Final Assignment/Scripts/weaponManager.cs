using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponManager : MonoBehaviour
{

    public static weaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;

    [Header("Throwables")]
    public float throwForce = 10f;

    public GameObject grenadePrefab;

    public GameObject throwableSpawn;
    public float forceMultiplier = 0;
    public float forceMultiplierLimit = 2f;

    public int lethalsCount = 0;
    public Throwables.ThrowableType equippedLethalType;

    public int tacticalsCount = 0;
    public Throwables.ThrowableType equippedTacticalType;
    public GameObject flashGrenadePrefab;
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

    public void Start()
    {
        activeWeaponSlot = weaponSlots[0];

        equippedLethalType = Throwables.ThrowableType.None;
        equippedTacticalType = Throwables.ThrowableType.None;
    }

    public void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if(weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            switchWeaponSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            switchWeaponSlot(1);
        }
        if (Input.GetKey(KeyCode.G)|| Input.GetKey(KeyCode.T))
        {
            forceMultiplier += Time.deltaTime;
            if (forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }
        if(Input.GetKeyUp(KeyCode.G))
        {
            if (lethalsCount > 0)
            {
                throwLethal();
            }

            forceMultiplier = 0;
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            if (tacticalsCount > 0)
            {
                throwTactical();
            }

            forceMultiplier = 0;
        }
    }

    public void pickUpWeapon(GameObject pickedUpWeapon)
    {
        addWeaponIntoActiveSlot(pickedUpWeapon);
    }

    private void addWeaponIntoActiveSlot(GameObject pickedUpWeapon)
    {
        dropCurrentWeapon(pickedUpWeapon);

        pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapons weapons = pickedUpWeapon.GetComponent<Weapons>();

        pickedUpWeapon.transform.localPosition = new Vector3(weapons.spawnPosition.x, weapons.spawnPosition.y, weapons.spawnPosition.z);
        pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapons.spawnRotation.x, weapons.spawnRotation.y, weapons.spawnRotation.z);

        weapons.isActiveWeapon = true;
        weapons.animator.enabled = true;
    }

    internal void pickUpAmmo(ammoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case ammoBox.AmmoType.pistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case ammoBox.AmmoType.rifleAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;
        }
    
    }

    private void dropCurrentWeapon(GameObject pickedUpWeapon)
    {
        if (activeWeaponSlot.transform.childCount >0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapons>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapons>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedUpWeapon.transform.localRotation;
        }
    }

    public void pickUpThrowable(Throwables throwables)
    {
        switch (throwables.throwabletype) 
        {
            case Throwables.ThrowableType.Grenade:
                pickupThrowableAsLethal(Throwables.ThrowableType.Grenade);
                break;
            case Throwables.ThrowableType.Flash:
                pickUpThrowableAsTactical(Throwables.ThrowableType.Flash);
                break;

        }
    }

    private void pickUpThrowableAsTactical(Throwables.ThrowableType tactical)
    {
        if (equippedTacticalType == tactical || equippedTacticalType == Throwables.ThrowableType.None)
        {
            equippedTacticalType = tactical;
            if (tacticalsCount < 2)
            {
                tacticalsCount += 1;
                Destroy(interactionManager.Instance.hoveredOverThrowable.gameObject);
                HUDManager.Instance.updateThrowables();
            }
            else
            {
                print("Limit tacticals reached");
            }
        }
        else
        {
            //cant pickup different lethal
        }
    }

    private void pickupThrowableAsLethal(Throwables.ThrowableType lethal)
    {
        if (equippedLethalType==lethal || equippedLethalType == Throwables.ThrowableType.None)
        {
            equippedLethalType = lethal;
            if (lethalsCount < 2)
            {
                lethalsCount += 1;
                Destroy(interactionManager.Instance.hoveredOverThrowable.gameObject);
                HUDManager.Instance.updateThrowables();
            }else
            {
                print("Limit lethals reached");
            }
        }else
        {
            //cant pickup different lethal
        }
    }
    private void throwTactical()
    {
        GameObject tacticalPrefab = GetThrowablePrefab(equippedTacticalType);

        GameObject throwable = Instantiate(tacticalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwables>().hasBeenThrown = true;

        tacticalsCount -= 1;

        if (tacticalsCount <= 0)
        {
            equippedTacticalType = Throwables.ThrowableType.None;
        }

        HUDManager.Instance.updateThrowables();
    }

    private void throwLethal()
    {
        GameObject lethalPrefab = GetThrowablePrefab(equippedLethalType);

        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwables>().hasBeenThrown = true;

        lethalsCount -= 1;

        if (lethalsCount <= 0)
        {
            equippedLethalType = Throwables.ThrowableType.None;
        }

        HUDManager.Instance.updateThrowables();
    }

    private GameObject GetThrowablePrefab(Throwables.ThrowableType throwableType)
    {
        switch (throwableType)
        {
            case Throwables.ThrowableType.Grenade:
                return grenadePrefab;
            case Throwables.ThrowableType.Flash:
                return flashGrenadePrefab;
        }
        return new();
    }

    public void switchWeaponSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapons currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapons>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapons newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapons>();
            newWeapon.isActiveWeapon = true;
        }

    }

    internal void decreaseTotalAmmo(int bulletsToDecrease, Weapons.weaponChoice thisWeaponChoice)
    {
        switch (thisWeaponChoice)
        {
            case Weapons.weaponChoice.ARAK47:
                totalRifleAmmo -= bulletsToDecrease;
                break;
            case Weapons.weaponChoice.Pistol1911:
                totalPistolAmmo -= bulletsToDecrease;
                break;
        }
    }

    public int checkAmmoLeftFor(Weapons.weaponChoice thisWeaponChoice)
    {
        switch (thisWeaponChoice)
        {
            case Weapons.weaponChoice.ARAK47:
                return Instance.totalRifleAmmo;
            case Weapons.weaponChoice.Pistol1911:
                return Instance.totalPistolAmmo;

            default:
                return 0;
        }
    }
}
