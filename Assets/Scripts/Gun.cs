using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    private float nextTimeToFire = 0;
    private bool isReloading = false;
    private int defaultFieldOfView = 60;
    private Camera camComponent;
    private Transform camTransform;
    private InputAction shoot;
    private InputAction zoom;

    public GameObject fpsCam;
    public float range = 20;
    public float impactForce = 150;
    public int fireRate = 10;
    public int fieldOfView = 60;
    /// <summary>
    /// Текущие патроны
    /// </summary>
    public int currentAmmo;
    /// <summary>
    /// Патронов в магазине
    /// </summary>
    public int magazineSize = 10;
    /// <summary>
    /// Остаток патронов
    /// </summary>
    public int restAmmo = 30;
    public float reloadTime = 2;  
    public int damageAmount = 20;
    public GameObject impactEffect;
    public ParticleSystem muzzleFlush;
    public Animator animator;

    void Start()
    {
        shoot = new InputAction("Shoot", binding: "<mouse>/leftButton");
        zoom = new InputAction("Zoom", binding: "<mouse>/rightButton");

        shoot.Enable();
        zoom.Enable();

        currentAmmo = magazineSize;

        camComponent = fpsCam.GetComponent<Camera>();
        camTransform = fpsCam.transform;

    }

    private void OnEnable()
    {
        animator.SetBool("isReloading", false);
        isReloading = false;
    }

    void Update()
    {

        camComponent.fieldOfView = zoom.ReadValue<float>() == 1 ? fieldOfView : defaultFieldOfView;

        if (currentAmmo == 0 && restAmmo == 0)
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

        if (!isReloading && restAmmo > 0 && currentAmmo != magazineSize)
            if (currentAmmo == 0 || Input.GetKeyDown(KeyCode.R))
                StartCoroutine(Reload());

    }

    private void Fire()
    {
        muzzleFlush.Play();

        currentAmmo--;
        
        if (Physics.Raycast(camTransform.position, camTransform.forward, out RaycastHit hit, range))
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

        int needAmmo = magazineSize - currentAmmo;

        animator.SetBool("isReloading", false);
        if (restAmmo >= needAmmo)
        {
            currentAmmo = magazineSize;
            restAmmo -= needAmmo;
        }
        else
        {
            currentAmmo += restAmmo;
            restAmmo = 0;
        }

        isReloading = false;
    }
}
