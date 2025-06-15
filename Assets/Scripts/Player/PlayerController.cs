using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]

public class PlayerController : NetworkBehaviour
{
    private NetworkCharacterControllerCustom _characterMovement;
    private WeaponHandler _weaponHandler;
    
    public override void Spawned()
    {
        _characterMovement = GetComponent<NetworkCharacterControllerCustom>();
        _weaponHandler = GetComponent<WeaponHandler>();
    }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out NetworkInputData inputs)) return;
        
        //Movimiento
        Vector3 moveDirection = Vector3.forward * inputs.movementInput;
        _characterMovement.Move(moveDirection);
        
        //Salto
        if (inputs.networkButtons.IsSet(MyButtons.Jump))
        {
            _characterMovement.Jump();
        }
        
        //Disparo
        //if (inputs.isFirePressed)
        //{
        //    _weaponHandler.Fire();
        //}
    }
}