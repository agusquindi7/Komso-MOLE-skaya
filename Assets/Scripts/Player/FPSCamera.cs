using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    Vector2 _turn;
    [Header("Values")]
    [SerializeField] float _sensitivity;
    [SerializeField, Range(0, 90f)] float _clampViewY = 90f;
    [SerializeField] float clampMin, clampMax;
    //[SerializeField] bool _isLocked = true;
    [SerializeField] Transform body;
    [SerializeField] Animator anim;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (anim == null) anim = GetComponentInParent<Animator>();

        PauseManager.instance.Subscribe(ArtificialUpdate);
    }

    //private void Start()
    //{
    //    if (_isLocked) Cursor.lockState = CursorLockMode.Locked;
    //    else Cursor.lockState = CursorLockMode.None;
    //}

    private void ArtificialUpdate()
    {
        _turn.x += (Input.GetAxisRaw("Mouse X") * _sensitivity);
        _turn.y += (Input.GetAxisRaw("Mouse Y") * -_sensitivity);
        _turn.y = Mathf.Clamp(_turn.y, -_clampViewY, _clampViewY); //Clamps the rotation vertically
        
        transform.rotation = Quaternion.Euler(_turn.y, _turn.x, 0);
        body.transform.localRotation = Quaternion.Euler(0 , _turn.x, 0);
    }

    private void OnDestroy()
    {
        PauseManager.instance.Unsubscribe(ArtificialUpdate);
    }
}
