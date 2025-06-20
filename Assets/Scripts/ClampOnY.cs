using UnityEngine;

public class ClampOnY : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        // Mirar hacia la cámara sin invertir la posición
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}