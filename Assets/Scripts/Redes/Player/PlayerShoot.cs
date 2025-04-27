using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform spawnerEmpty;

    void Update()
    {
        //Vector3 mousePos = Input.mousePosition;
        //Vector2 dir = new Vector2(mousePos.x, mousePos.y);

        //transform.rotation = Quaternion.Euler(0f, 0f, angle);






        //Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, spawnerEmpty.position.z));

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (plane.Raycast(ray, out float enter))
        //{
        //    Vector3 worldPos = ray.GetPoint(enter);

        //    Vector3 dir = worldPos - spawnerEmpty.position;
        //    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //    spawnerEmpty.rotation = Quaternion.Euler(0f, 0f, angle);
        //}

        //spawnerEmpty.rotation = spawnerEmpty.LookAt(mousePos);



        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = -Camera.main.transform.position.z; //anulo el eje z
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);//transformo a sistema de coordenada

        Vector2 dir = (mouseWorld - spawnerEmpty.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        spawnerEmpty.rotation = Quaternion.Euler(0f, 0f, angle);

    }
}
