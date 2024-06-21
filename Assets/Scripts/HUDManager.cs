using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
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
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol1911:
                return Instantiate(Resources.Load<GameObject>("Pistol1911_Weapon")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.Uzi:
                return Instantiate(Resources.Load<GameObject>("Uzi_Weapon")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.AK74:
                return Instantiate(Resources.Load<GameObject>("AK74_Weapon")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.M249:
                return Instantiate(Resources.Load<GameObject>("M249_Weapon")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.M107:
                return Instantiate(Resources.Load<GameObject>("M107_Weapon")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.Bennelli_M4:
                return Instantiate(Resources.Load<GameObject>("BenelliM4_Weapon")).GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol1911:
                return Instantiate(Resources.Load<GameObject>("Pistol_Ammo")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.Uzi:
                return Instantiate(Resources.Load<GameObject>("Bullet9mm_Ammo")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.AK74:
                return Instantiate(Resources.Load<GameObject>("Rifle_Ammo")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.Bennelli_M4:
                return Instantiate(Resources.Load<GameObject>("ShotGun_Ammo")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.M249:
                return Instantiate(Resources.Load<GameObject>("Rifle_Ammo")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.M107:
                return Instantiate(Resources.Load<GameObject>("Sniper_Ammo")).GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }
    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        //This will never happen, but we need to return something
        return null;
    }
}
