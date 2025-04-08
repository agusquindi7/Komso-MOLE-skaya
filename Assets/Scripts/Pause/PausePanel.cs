using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    bool isON = false;
    public GameObject panel;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isON = !isON;
            PauseManager.instance.Pause(isON);
            panel.SetActive(isON);
        }
    }

    public void SceneLoad(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
