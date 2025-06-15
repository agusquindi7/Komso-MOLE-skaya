using System;
using Fusion;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef _bulletPrefab;
    [SerializeField] private Transform _shotSpawnTransform;

    public event Action OnShot = delegate { };

    public void Fire()
    {
        if (!HasStateAuthority) return;
        
        #region Spawn Bullet

        //Runner.Spawn(_bulletPrefab, _shotSpawnTransform.position, _shotSpawnTransform.rotation);
        
        RayBullet();
        
        #endregion
    }
    
    void RayBullet()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 2, Color.magenta, 2);

        Runner.LagCompensation.Raycast(transform.position, transform.forward, 100f, Object.InputAuthority, out var hitInfo);
        
        if (hitInfo.Hitbox == null) return;

        if (!hitInfo.Hitbox.transform.root.TryGetComponent(out LifeHandler player)) return;
        
        player.TakeDamage(25);
    }
}