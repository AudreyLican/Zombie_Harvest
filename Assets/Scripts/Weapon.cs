using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true; //allow reset only once
    public float shootingDelay = 2f;

    //Burst (mode)
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //Spread
    public float spreadIntensity;

    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn; //Poistion where the bullet will be instantiate
    public float bulletVelocity = 30; //bullet speed
    public float bulletPrefabLifeTime = 3f; // seconds
    //Muzzle Effect (when player shoot)
    public GameObject muzzleEffect;
    private Animator animator;

    //Loading
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;// keep track is player is reloarding

   public enum WeaponModel
    {
        Pistol1911,
        AK74,
        Uzi,
        Bennelli_M4
    }

    public WeaponModel thisWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true; // because at the begenning player will be ready to shoot
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
    }

    void Update()
    {
        //play empty sound magazine if shooting and weapon empty
        if (bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.emptyMagazineSoundM1911.Play();
        }

        //considering the different shooting mode :

        if(currentShootingMode == ShootingMode.Auto)
        {
            //Holding Down Left Mouse Button
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.Single ||
                currentShootingMode == ShootingMode.Burst)
        {
            //Clicking Left Mouse Button Once
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        //Reloading when player press R
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false)
        {
            Reload();
        }

        //Automatically reload when magazine is empty
        if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <=0)
        {
            //Reload();
        }


        if (readyToShoot && isShooting && bulletsLeft > 0)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }

        //updating UI according to the bullet left
        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft/bulletsPerBurst}/{magazineSize/bulletsPerBurst}";
        }
    }

    private void FireWeapon()
    {
        //decreasing bullet left for each shoot
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");

        /**
         * SoundManager.Instance.shootingSoundM1911.Play();
         * New method, instead of single handling each sound in code
         * SoundManager have been change so that he check which weaopon have been selected
         * and play the right sound
         * Calling in SoundManager Instance PlayShootingSound and pass in argument
         * enum thisWeaponModel and check if on list : Yes play sound accordingly, No play nothing
        */
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);
        
        readyToShoot = false; //when starting shooting, set value to false so that the player won't be able to shoot the second bullet when the first one is not finished

        
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized; //Shooting direction

        //Instatiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        // or (Test : we want rotation bullet if player want to shoot when rotate is camera)
        // GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        //Poiting the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;

        //Shoot the bullet & apply force
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        
        //Destroy bullet after some time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        //Check if we are done shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //Burst Mode
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) // we already shoot onece before this check
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    
    }

    private void Reload()
    {
        /**
         * Play reloading sound according to the selected weapon
         * 
         * old code from single handled each sound : SoundManager.Instance.reloadingSoundM1911.Play();
        */
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);

        animator.SetTrigger("RELOAD");

        isReloading = true;
        //for delay until reloading is complete
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        //Shooting from from the middle of the screen to check where are we pointing
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            //Hitting Something
            targetPoint = hit.point;
        }
        else
        {
            //Hitting at the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //Returning the shooting direction and spread 
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {

        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
