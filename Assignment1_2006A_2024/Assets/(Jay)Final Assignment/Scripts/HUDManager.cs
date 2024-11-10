using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image lethalsUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalsUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;

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
        Weapons activeWeapon = weaponManager.Instance.activeWeaponSlot?.GetComponentInChildren<Weapons>();
        Weapons unActiveWeapon = GetUnactiveWeaponSlot()?.GetComponentInChildren<Weapons>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletPerBurst}";
            totalAmmoUI.text = $"{activeWeapon.magSize / activeWeapon.bulletPerBurst}";

            Weapons.weaponChoice model = activeWeapon.thisWeaponChoice;
            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(model);
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";
            ammoTypeUI.sprite = emptySlot;
            activeWeaponUI.sprite = emptySlot;
        }

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponChoice);
            }else
            {
                unActiveWeaponUI.sprite = emptySlot;
            }
        
    }

    private Sprite GetWeaponSprite(Weapons.weaponChoice model)
    {
        switch (model)
        {
            case Weapons.weaponChoice.Pistol1911:
                return Resources.Load<GameObject>("Pistol1911_Weapon").GetComponent<SpriteRenderer>().sprite;

            case Weapons.weaponChoice.ARAK47:
                return Resources.Load<GameObject>("ARAK47_Weapon").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private Sprite GetAmmoSprite(Weapons.weaponChoice model)
    {
        switch (model)
        {
            case Weapons.weaponChoice.Pistol1911:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;

            case Weapons.weaponChoice.ARAK47:
                return Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private GameObject GetUnactiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in weaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != weaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            } 
        }
        return null;
    }
}