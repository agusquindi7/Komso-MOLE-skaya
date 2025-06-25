using UnityEngine;
using Fusion;

public class AnimationManager : NetworkBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] float raycastDistance;
    [SerializeField] NetworkObject parent;
    [SerializeField] NetworkMecanimAnimator netAnimator;

    public override void FixedUpdateNetwork()
    {
        if (!HasInputAuthority) return;

        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance);
        RPC_SetBool("isGrounded", isGrounded);

        float h = Input.GetAxisRaw("Horizontal");
        bool isRunning = isGrounded && Mathf.Abs(h) > 0.1f;
        RPC_SetBool("isRunning", isRunning);

        if (isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            RPC_SetTrigger("Jump");
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    void RPC_SetBool(string param, bool value)
    {
        anim.SetBool(param, value);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    void RPC_SetTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
        netAnimator.SetTrigger(triggerName); // Trigger sí necesita replicación manual
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * raycastDistance);
    }
}
