using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Fixed
 */
public class InteractionManager : MonoBehaviour
{
    //This sigleton, allow to have access to this manager anywhere inside project
    public static InteractionManager Instance { get; set; }

    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    public Throwable hoveredThrowable = null;

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

    /**
     * Make Interaction with Game Object : Weapon, Ammo, Throwable(grenable, flash, bomb...)
     * 
     */
    private void Update()
    {
        // Check what the player is hitting

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            if (objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                // Disable the outline of previously selected item (avoid multiple thing selected)
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }

                hoveredWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                    hoveredWeapon = null; // ---- CHECK NULL
                }
                
            }

            // AmmoBox
            if (objectHitByRaycast != null && objectHitByRaycast.GetComponent<AmmoBox>()) // ---- CHECK NULL
            {
                // Disable the outline of previously selected item
                if (hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }

                hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
                hoveredAmmoBox.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                    Destroy(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }

            }

            //Throwable
            if (objectHitByRaycast != null && objectHitByRaycast.GetComponent<Throwable>()) // ---- CHECK NULL
            {
                // Disable the outline of previously selected item
                if (hoveredThrowable)
                {
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }

                hoveredThrowable = objectHitByRaycast.gameObject.GetComponent<Throwable>();
                hoveredThrowable.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupThrowable(hoveredThrowable);
                    GrenadeGrape grapeGrenade = hoveredThrowable.transform.parent.GetComponent<GrenadeGrape>();
                    if (grapeGrenade != null)
                    {
                        hoveredThrowable.gameObject.SetActive(false); //simulate pickup
                        grapeGrenade.CheckGrenades();
                    }
                }
            }
            else
            {
                if (hoveredThrowable)
                {
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }

            }
        }
    }
}
