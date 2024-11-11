using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapons;

public class soundManager : MonoBehaviour
{
    public static soundManager Instance { get; set; }

    public AudioSource shootingChannel;

    public AudioClip P1911Shot;
    public AudioClip AK47Shot;

    public AudioSource reloadSound1911;
    public AudioSource reloadSoundARAK47;

    public AudioSource emptyMagSound1911;

    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;

    public AudioClip zombieWalking;
    public AudioClip zombieHurt;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieDeath;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;

    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDead;
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

    public void playShootingSound(weaponChoice weapon)
    {
        switch(weapon)
        {
            case weaponChoice.Pistol1911:
                shootingChannel.PlayOneShot(P1911Shot);
                break;
            case weaponChoice.ARAK47:
                shootingChannel.PlayOneShot(AK47Shot);
                break;
        }
    }

    public void playReloadSound(weaponChoice weapon)
    {
        switch (weapon)
        {
            case weaponChoice.Pistol1911:
                reloadSound1911.Play();
                break;
            case weaponChoice.ARAK47:
                reloadSoundARAK47.Play();
                break;
        }

    }
}
