using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform spawnerEmpty;
    public Transform spawnerBullet;
    public GameObject bullet;
    public float cdMax = 2f;
    public float cd;
    public KeyCode keyShoot = KeyCode.Mouse0;


    void Update()
    {
        RotateSpawner();
        if (Input.GetKeyDown(keyShoot))
        {
            if (cd >= cdMax)
            {
                Instantiate(bullet, spawnerBullet.position, spawnerBullet.rotation);

                cd = 0;
            }
        }

        cd += Mathf.Clamp(Time.deltaTime, 0, 3);

    }

    public void RotateSpawner()
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
