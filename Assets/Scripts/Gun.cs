using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    private float nextTimeToFire = 0;
    private bool isReloading = false;
    private InputAction shoot;

    public Transform fpsCam;
    public float range = 20;
    public float impactForce = 150;
    public int fireRate = 10;  
    public int currentAmmo;
    public int maxAmmo = 10;
    public int magazineSize = 30;
    public float reloadTime = 2;  
    public int damageAmount = 20;
    public GameObject impactEffect;
    public ParticleSystem muzzleFlush;
    public Animator animator;

    void Start()
    {
        shoot = new InputAction("Shoot", binding: "<mouse>/leftButton");
        shoot.Enable();

        currentAmmo = maxAmmo;

    }

    private void OnEnable()
    {
        animator.SetBool("isReloading", false);
        isReloading = false;
    }

    void Update()
    {

        if (currentAmmo == 0 && magazineSize == 0)
        {
            animator.SetBool("isShooting", false);
            return;
        }

        if (isReloading)
            return;

        bool isShooting = shoot.ReadValue<float>() == 1;
        animator.SetBool("isShooting", isShooting);

        if (isShooting && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
        }

        if (!isReloading && magazineSize > 0 && currentAmmo != maxAmmo)
            if (currentAmmo == 0 || Input.GetKeyDown(KeyCode.R))
                StartCoroutine(Reload());
    }

    private void Fire()
    {
        muzzleFlush.Play();

        currentAmmo--;
        
        if (Physics.Raycast(fpsCam.position, fpsCam.forward, out RaycastHit hit, range))
        {
            if (hit.rigidbody != null)
                hit.rigidbody.AddForce(-hit.normal * impactForce);

            Enemy enemy = hit.transform.GetComponent<Enemy>();

            if (enemy != null)
                enemy.TakeDamage(damageAmount);

            Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
            GameObject impact = Instantiate(impactEffect, hit.point, impactRotation);
            Destroy(impact, 5);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("isReloading", true);

        yield return new WaitForSeconds(reloadTime);

        int bulletCount = maxAmmo - currentAmmo;

        animator.SetBool("isReloading", false);
        if (magazineSize >= bulletCount)
        {
            currentAmmo = maxAmmo;
            magazineSize -= bulletCount;
        }
        else
        {
            currentAmmo += magazineSize;
            magazineSize = 0;
        }

        isReloading = false;
    }
}
