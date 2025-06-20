using UnityEngine;
using Fusion;

public class ShieldController : NetworkBehaviour
{
    [SerializeField] private GameObject _shieldChild;
    public KeyCode shieldKey = KeyCode.LeftShift;

    [Networked, OnChangedRender(nameof(ActivateShield))]
    private bool IsShieldActive { get; set; }

    private void ActivateShield()
    {

        _shieldChild.SetActive(IsShieldActive);
    }

    public override void FixedUpdateNetwork()
    {
        if (HasInputAuthority)
        {
            bool input = Input.GetKey(shieldKey);

            // Si el input cambió, avisamos al StateAuthority con un RPC
            if (input != IsShieldActive)
            {
                RPC_RequestShieldChange(input);
            }
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_RequestShieldChange(bool active)
    {
        //Debug.Log($" HOST ejecutó RPC del jugador {Object.InputAuthority} -> {active}");
        IsShieldActive = active;
    }

    public override void Spawned()
    {
        _shieldChild.SetActive(IsShieldActive);
    }
}
