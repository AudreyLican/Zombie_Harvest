using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    // This is a singleton
    public static SoundManager Instance { get; set; }

    //Weapons
    public AudioSource ShootingChannel;
    
    public AudioClip M1911Shot;
    public AudioClip AK74Shot;
    public AudioClip UziShot;
    public AudioClip M107Shot;
    //public AudioClip M249Shot;
    public AudioClip M4_8Shot;
    public AudioClip BenelliM4Shot;

    public AudioSource reloadingSoundM1911;
    public AudioSource reloadingSoundAK74;
    public AudioSource reloadingSoundM4_8;
    public AudioSource reloadingSoundUzi;
    public AudioSource reloadingSoundBennelliM4;
    public AudioSource reloadingSoundM107;
    //public AudioSource reloadingSoundM249;

    public AudioSource emptyMagazineSoundM1911;

    // Throwables
    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;
    public AudioClip smokeGrenadeSound;

    // Zombies
    public AudioSource zombieChannel;
    public AudioSource zombieChannel2; // avoir conflit between 2 sound that should be heard in the same time
    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieDeath;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;

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

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                ShootingChannel.PlayOneShot(M1911Shot);
                break;
            case WeaponModel.AK74:
                ShootingChannel.PlayOneShot(AK74Shot);
                break;

            case WeaponModel.M4_8:
                ShootingChannel.PlayOneShot(M4_8Shot);
                break;

            case WeaponModel.Uzi:
                ShootingChannel.PlayOneShot(UziShot);
                break;
            case WeaponModel.Bennelli_M4:
                ShootingChannel.PlayOneShot(BenelliM4Shot);
                break;
            case WeaponModel.M107:
                ShootingChannel.PlayOneShot(M107Shot);
                break;
            case WeaponModel.M249:
                //ShootingChannel.PlayOneShot(M249Shot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                reloadingSoundM1911.Play();
                break;
            case WeaponModel.AK74:
                reloadingSoundAK74.Play();
                break;

            case WeaponModel.M4_8:
                reloadingSoundM4_8.Play();
                break;

            case WeaponModel.Uzi:
                reloadingSoundUzi.Play();
                break;
            case WeaponModel.Bennelli_M4:
                reloadingSoundBennelliM4.Play();
                break;

            case WeaponModel.M107:
                reloadingSoundM107.Play();
                break;
            case WeaponModel.M249:
                //reloadingSoundM249.Play();
                break;
        }
    }
}
