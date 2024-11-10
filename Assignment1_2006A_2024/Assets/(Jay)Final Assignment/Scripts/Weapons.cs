using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapons : MonoBehaviour
{
    public bool isActiveWeapon;

    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;
    //3 round burst
    public int bulletPerBurst = 3;
    public int currentBurst;
    //Spread
    public float spreadIntensity;
    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVeloctiy = 30;
    public float bulletPrefabLifeTime = 3f;

    //muzzle
    public GameObject muzzleEffect;
    internal Animator animator;

    //reloading
    public float reloadTime;
    public int magSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    public enum weaponChoice
    {
        Pistol1911,
        ARAK47
    }

    public weaponChoice thisWeaponChoice;

    public enum shootingMode
    {
        SemiAuto,
        Burst,
        Auto
    }

    public shootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        currentBurst = bulletPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magSize;
    }

    // Update is called once per frame
    void Update()
    {

        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;
            if (bulletsLeft == 0 && isShooting)
            {
                soundManager.Instance.emptyMagSound1911.Play();
            }

            if (currentShootingMode == shootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);//holding left click
            }
            else if (currentShootingMode == shootingMode.SemiAuto ||
                currentShootingMode == shootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && isReloading == false && weaponManager.Instance.checkAmmoLeftFor(thisWeaponChoice)>0)
            {
                reload();
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                currentBurst = bulletPerBurst;
                FireWeapon();
            }

        }
    }

    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");

        //soundManager.Instance.shootingSound1911.Play();
        soundManager.Instance.playShootingSound(thisWeaponChoice);

        readyToShoot = false;

        Vector3 shootingDirection = calculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVeloctiy, ForceMode.Impulse);

        StartCoroutine(destroyBullet(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke("resetShot", shootingDelay);
            allowReset = false; 
        }

        if (currentShootingMode == shootingMode.Burst && currentBurst > 1)
        {
            currentBurst--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void reload()
    {
        //soundManager.Instance.reloadSound1911.Play();
        soundManager.Instance.playReloadSound(thisWeaponChoice);
        animator.SetTrigger("RELOAD");

        isReloading = true;
        Invoke("reloadCompleted", reloadTime);
    }

    private void reloadCompleted()
    {
        if(weaponManager.Instance.checkAmmoLeftFor(thisWeaponChoice)>magSize)
        {
            bulletsLeft = magSize;
            weaponManager.Instance.decreaseTotalAmmo(bulletsLeft, thisWeaponChoice);
        }else
        {
            bulletsLeft = weaponManager.Instance.checkAmmoLeftFor(thisWeaponChoice);
            weaponManager.Instance.decreaseTotalAmmo(bulletsLeft, thisWeaponChoice);
        }

        isReloading = false;
    }

    private void resetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 calculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }
    private IEnumerator destroyBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
