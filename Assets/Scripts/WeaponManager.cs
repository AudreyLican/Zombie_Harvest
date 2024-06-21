using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

/**
 * Singleton :
 * Script that control all the weapons :
 * how to deal with it, switch between them...
 * Manage weapon being pickedup and put in the weaponslot, position set to player vision, 
 * Scenario : weapon on map switch between weapon in the player hand
 */
public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;
    public int totalShotGunAmmo = 0;
    public int totalSniperAmmo = 0;
    public int totalBullet9mmAmmo = 0;

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

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
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
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }
    }

    public void PickupWeapon(GameObject pickedupWeapon)
    {
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedupWeapon)
    {
        DropCurrentWeapon(pickedupWeapon);

        pickedupWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>();

        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        pickedupWeapon.transform.localScale = new Vector3(weapon.spawnScale.x, weapon.spawnScale.y, weapon.spawnScale.z); //fixe scale of weapon for each pickup

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    //Check Ammo and Add it
    internal void PickupAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.RifleAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.ShotGunAmmo:
                totalShotGunAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.SniperAmmo:
                totalSniperAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.Bullet9mmAmmo:
                totalBullet9mmAmmo += ammo.ammoAmount;
                break;
        }
    }

    /**
     * Weapon drop to be switch to new one, in game
     * value position and rotation, are being switch
     */
    public void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedupWeapon.transform.localRotation;
            weaponToDrop.transform.localScale = pickedupWeapon.transform.localScale; //fixe scale of weapon for each pickup
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }

    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.Pistol1911:
                totalPistolAmmo -= bulletsToDecrease;
                break;

            case Weapon.WeaponModel.Bennelli_M4:
                totalShotGunAmmo -= bulletsToDecrease;
                break;

            case Weapon.WeaponModel.Uzi:
                totalBullet9mmAmmo -= bulletsToDecrease;
                break;

            case Weapon.WeaponModel.M249:
                totalRifleAmmo -= bulletsToDecrease;
                break;

            case Weapon.WeaponModel.M107:
                totalSniperAmmo -= bulletsToDecrease;
                break;

            case Weapon.WeaponModel.AK74:
                totalRifleAmmo -= bulletsToDecrease;
                break;
        }
    }

    public int CheckAmmoLeftFor(Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.Pistol1911:
                return totalPistolAmmo;

            case Weapon.WeaponModel.Uzi:
                return totalBullet9mmAmmo;

            case Weapon.WeaponModel.M249:
                return totalRifleAmmo;

            case Weapon.WeaponModel.M107:
                return totalSniperAmmo;

            case Weapon.WeaponModel.Bennelli_M4:
                return totalShotGunAmmo;

            default:
                return 0;
        }
    }
}
