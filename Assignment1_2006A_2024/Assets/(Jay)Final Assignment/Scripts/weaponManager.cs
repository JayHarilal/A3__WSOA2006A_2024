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