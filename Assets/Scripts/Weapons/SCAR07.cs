using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCAR07 : Weapons
{
    private void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (muzzleFlash == null) muzzleFlash = GetComponentInChildren<ParticleSystem>();
        if (audioSourceWeapon == null) audioSourceWeapon = GetComponentInChildren<AudioSource>();
        if (movement == null) movement = GetComponentInParent<PlayerMovement>();
    }
}
