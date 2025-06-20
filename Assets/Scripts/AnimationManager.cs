using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] float raycastDistance;

    private void Update()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance);
        anim.SetBool("isGrounded", isGrounded);
        Debug.Log(isGrounded);

        if (isGrounded) //SI ESTOY EN EL PISO CHEQUEA SI ESTOY CORRIENDO
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                anim.SetBool("isRunning", true);
            }
            else anim.SetBool("isRunning", false);
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.W))
            anim.SetTrigger("Jump");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, new Vector3(0,-1 * raycastDistance,0));
    }
}
