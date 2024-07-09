using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GrenadeGrape : MonoBehaviour
{
    public List<GameObject> grenades; //List of grenades objects
    public Light spotLight; // refere to spotlight
    void Start()
    {
        spotLight = GetComponentInChildren<Light>();
        /*
        if (spotLight != null)
        {
            Debug.LogError("No spotLight found on the GrapeGrenade object.");
        }*/

        // Ensure kinematic is disabled for all grenades at start (assuming they are kinematic by default)
        foreach (GameObject grenadeObj in grenades)
        {
            Rigidbody rb = grenadeObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Disable physics initially
                rb.useGravity = false; // Disable gravity initially
            }
        }

        CheckGrenades(); // Initialize the spotlight state
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickupGrenades();
        }
    }

    private void PickupGrenades()
    {
        foreach (GameObject grenadeObj in grenades)
        {
            Throwable throwable = grenadeObj.GetComponent<Throwable>();
            if (throwable != null && !throwable.pickedUp)
            {
                throwable.pickedUp = true;
                WeaponManager.Instance.PickupThrowable(throwable); // Ensure throwable is picked up
            }
        }
        CheckGrenades(); // Update the spotlight state
    }

    public void CheckGrenades()
    {
        bool allGrenadesPickedUp = true;

        // Check if all grenades have been picked up
        foreach (GameObject grenadeObj in grenades)
        {
            Throwable throwable = grenadeObj.GetComponent<Throwable>();
            if (throwable != null && !throwable.pickedUp)
            {
                allGrenadesPickedUp = false;
                break;
            }
        }

        // Enable or disable the spotlight based on whether all grenades are picked up
        if (spotLight != null)
        {
            spotLight.enabled = !allGrenadesPickedUp;
        }
    }
}
