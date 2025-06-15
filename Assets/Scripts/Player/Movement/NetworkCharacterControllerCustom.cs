using Fusion;
using UnityEngine;

public class NetworkCharacterControllerCustom : NetworkCharacterController
{
    public override void Move(Vector3 direction)
    {
        var deltaTime    = Runner.DeltaTime;
        var previousPos  = transform.position;
        var moveVelocity = Velocity;

        direction = direction.normalized;

        //Data.Grounded -> Grounded
        if (Grounded && moveVelocity.y < 0) {
            moveVelocity.y = 0f;
        }

        moveVelocity.y += gravity * Runner.DeltaTime;

        var horizontalVel = default(Vector3);
        //horizontalVel.x = moveVelocity.x; //Not used
        horizontalVel.z = moveVelocity.x; //horizontalVel.Z = moveVelocity.X instead of moveVelocity.z


        if (direction == default) 
        {
            horizontalVel = Vector3.Lerp(horizontalVel, default, braking * deltaTime);
        } 
        else 
        {
            horizontalVel      = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, maxSpeed);
            transform.rotation = Quaternion.Euler(Vector3.up * (Mathf.Sign(direction.z) < 0 ? -90 : 90));//Instead of a Slerp
        }

        moveVelocity.x = horizontalVel.z; //moveVelocity.X = horizontalVel.Z instead of horizontalVel.x
        //moveVelocity.z = horizontalVel.z; //Not Used


        Controller.Move(moveVelocity * deltaTime);

        Velocity = (transform.position - previousPos) * Runner.TickRate;//Data.Velocity -> Velocity
        Grounded = Controller.isGrounded;//Data.Grounded -> Grounded


   
    }
}