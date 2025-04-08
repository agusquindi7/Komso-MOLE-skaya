using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapons : MonoBehaviour
{
    [Header("Weapon Values")]
    public int maxAmmo;
    public int currentAmmo;
    public float fireRate;
    float counter;
    public float reloadTime;
    private bool isShooting;
    private bool canShoot;

    [Header("Component references")]
    [SerializeField] protected Animator anim;
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected AudioSource audioSourceWeapon;
    [SerializeField] protected PlayerMovement movement;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        counter += Time.deltaTime;
        counter = Mathf.Clamp(counter, 0, fireRate);

        Debug.Log(counter);

        if (Input.GetMouseButton(0) && counter >= fireRate)
        {
            Shoot();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            muzzleFlash.Stop();
        }

        else if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
            Debug.Log("RECARGO");
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            Aim();
            Debug.Log("APUNTO");
        }
        else if (movement.isGrounded && Input.GetAxisRaw("Horizontal")!= 0)
        {
            WalkWithWeapon();
            Debug.Log("WALK");
        }
        else if (movement.isGrounded && Input.GetAxisRaw("Vertical") != 0)
        {
            WalkWithWeapon();
            Debug.Log("WALK");
        }
        else
        {
            Idle();
            muzzleFlash.Stop();
            Debug.Log("IDLE");
        }
    }

    public void Idle()
    {
        anim.SetBool("Shoot", false);
        anim.SetBool("Idle", true);
    }

    public void WalkWithWeapon()
    {
        anim.SetBool("Shoot", false);
        anim.SetBool("Idle", false);
    }

    public void Aim()
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Aim", true);
    }

    public virtual void Shoot()
    {
        counter = 0;

        anim.SetBool("Shoot", true);
        muzzleFlash.Play();

        //LOGICA DE DISPARO (ES LA MISMA PARA TODAS LAS ARMAS)

    }

    public void Reload()
    {
        anim.SetBool("Shoot", false);
        anim.SetBool("Idle", false);
        anim.SetTrigger("Reload");
    }
}
