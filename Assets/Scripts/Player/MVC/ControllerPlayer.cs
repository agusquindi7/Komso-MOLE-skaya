using UnityEngine;

public class ControllerPlayer
{
    ModelPlayer _model;

    public ControllerPlayer(ModelPlayer model)
    {
        _model = model;
    }

    public void ArtificialUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        Vector3 dirRoll = new Vector3(horizontal, 0, vertical);

        if (Input.GetKeyDown(KeyCode.Space))
            _model.Jump();

        if(!_model.LandUpdate())
            _model.LandUpdate();

        if (!_model.RollUpdate())
            _model.RollUpdate();

        if (Input.GetKeyDown(KeyCode.LeftControl) && _model.RollUpdate())
            _model.Roll(dirRoll);
    }

    public void ArtificialFixed()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var turnX = Input.GetAxisRaw("Mouse X");
        var turnY = Input.GetAxisRaw("Mouse Y");

        if ((horizontal != 0 || vertical != 0) && Input.GetKey(KeyCode.LeftShift))
        {
            _model.Run(horizontal, vertical);
        }
        else if (horizontal != 0 || vertical != 0)
        {
            _model.Walk(horizontal, vertical);
        }
        else
        {
            _model.Idle();
        }

        _model.RotateCharacter();
       
    }
}
