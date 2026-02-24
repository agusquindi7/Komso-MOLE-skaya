using Fusion;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LifeHandler : NetworkBehaviour
{
    [Networked] public byte CurrentLife { get; set; }

    private const byte MAX_LIFE = 100;

    public Slider slider;

    public AudioSource audioSource;
    public AudioClip hitSound;
    
    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            CurrentLife = MAX_LIFE;
            slider.value = CurrentLife;
        }
    }

    private void Update()
    {
        UpdateIAUI();
    }

    public void TakeDamage(byte dmg)
    {
        if (dmg > CurrentLife) dmg = CurrentLife;

        CurrentLife -= dmg;

        RPCAudioHitSound();

        if (CurrentLife != 0) return;

        Debug.Log(Runner.LocalPlayer);
        GameManager.Instance.RPC_Defeat(Object.InputAuthority);

        //DisconnectPlayer();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPCAudioHitSound()
    {
        audioSource.PlayOneShot(hitSound, 1f);
    }

    //void DisconnectPlayer()
    //{
    //    if (!Object.HasInputAuthority)
    //    {
    //        Runner.Disconnect(Object.InputAuthority);
    //    }

    //    Runner.Despawn(Object);
    //}

    private void UpdateIAUI()
    {
        slider.value = (float)CurrentLife / (float)MAX_LIFE;
        //print($"{slider.value} = {(float)CurrentLife} / {(float)MAX_LIFE}");
    }
}