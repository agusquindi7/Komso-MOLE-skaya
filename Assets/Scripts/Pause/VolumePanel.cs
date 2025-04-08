using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumePanel : MonoBehaviour
{
    bool isON = false;
    public GameObject volumePanel, pausePanel;
    public void TurnVolumePanel()
    {
        isON = !isON;
        volumePanel.SetActive(isON);
        pausePanel.SetActive(!isON);
    }

    public void TurnPausePanel()
    {
        isON = !isON;
        volumePanel.SetActive(!isON);
        pausePanel.SetActive(isON);
    }
}
