using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]

public class PlayerController : NetworkBehaviour
{
    private NetworkCharacterControllerCustom _characterMovement;
    //[SerializeField] Transform spawnerEmpty;
    //private WeaponHandler _weaponHandler;
    
    public override void Spawned()
    {
        _characterMovement = GetComponent<NetworkCharacterControllerCustom>();
        //_weaponHandler = GetComponent<WeaponHandler>();
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

        //Rotacion del cañon
        //Vector3 mouseScreen = Input.mousePosition;
        //if (mouseScreen.magnitude != 0)
        //    _characterMovement.RotateCannon(mouseScreen, spawnerEmpty);

        //Disparo
        //if (inputs.isFirePressed)
        //{
        //    _weaponHandler.Fire();
        //}
    }
}