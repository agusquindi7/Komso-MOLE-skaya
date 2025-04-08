using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMVC : MonoBehaviour
{
    ModelPlayer _model;
    ViewPlayer _view;
    ControllerPlayer _controller;
    [SerializeField] float speed, jumpStrenght, rollStrenght, rollCD, maxLife, sensitivity, clampViewY;
    Vector2 _turn;
    float life, counterRoll;
    bool isGrounded, canRoll;
    [SerializeField] Rigidbody myRB;
    [SerializeField] Animator animator;
    [SerializeField] Transform pivot;

    private void Awake()
    {
        _model = new ModelPlayer(myRB, transform, speed, jumpStrenght, rollStrenght, maxLife, isGrounded, canRoll, counterRoll, rollCD              
                                ,sensitivity, clampViewY, _turn, pivot);
        _view = new ViewPlayer(_model, animator, transform);
        _controller = new ControllerPlayer(_model);
    }

    private void FixedUpdate()
    {
        _controller.ArtificialFixed();
    }

    public void Update()
    {
        _controller.ArtificialUpdate();
    }
}
