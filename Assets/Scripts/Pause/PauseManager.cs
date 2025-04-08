using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    public bool isPaused = false;
    public GameObject panel;
    public Action ArtificialUpdate;
    public Action ArtificialLate;

    public event Action TotalUpdates = delegate { };
    public event Action TotalLateUpdates = delegate { }; // Agregado para LateUpdate

    private void Awake()
    {
        ArtificialUpdate = TotalUpdates;
        ArtificialLate = TotalLateUpdates; // Inicializar con eventos vac�os

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Mas de una instancia de PauseManager detectada. Eliminando esta instancia.");
            Destroy(gameObject);
        }
    }

    public void Subscribe(Action callback, bool isLateUpdate = false)
    {
        if (isLateUpdate)
        {
            TotalLateUpdates += callback;
            ArtificialLate = TotalLateUpdates;
        }
        else
        {
            TotalUpdates += callback;
            ArtificialUpdate = TotalUpdates;
        }
    }

    public void Unsubscribe(Action callback, bool isLateUpdate = false)
    {
        if (isLateUpdate)
        {
            TotalLateUpdates -= callback;
            ArtificialLate = TotalLateUpdates;
        }
        else
        {
            TotalUpdates -= callback;
            ArtificialUpdate = TotalUpdates;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            Pause(isPaused);
        }

        ArtificialUpdate();
    }

    private void LateUpdate()
    {
        ArtificialLate();
    }

    public void Pause(bool active)
    {
        if (active)
        {
            panel.SetActive(active);
            Debug.Log("JUEGO PAUSADO");
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ArtificialUpdate = delegate { };
            ArtificialLate = delegate { }; // Asegurar que LateUpdate tambi�n se detiene
        }
        else
        {
            Time.timeScale = 1f;
            panel.SetActive(active);
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            ArtificialUpdate = TotalUpdates;
            ArtificialLate = TotalLateUpdates;
        }
    }
}
