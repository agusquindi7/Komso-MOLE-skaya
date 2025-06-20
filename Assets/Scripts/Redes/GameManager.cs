using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject _winImage;
    [SerializeField] private GameObject _loseImage;
    [SerializeField] private GameObject waitImage;
    private List<PlayerRef> _players = new List <PlayerRef>();
    public static GameManager Instance { get; private set; }

    public override void Spawned()
    {
        Debug.Log($"GameManager Spawned. StateAuthority: {Object.StateAuthority} | LocalPlayer: {Runner.LocalPlayer}");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //Debug.Log(Runner.SessionInfo.PlayerCount);
    }

    private void Update()
    {
        if (Runner == null) return;

        if (Runner.SessionInfo.PlayerCount < 1) //PARA TESTEAR LO PUSE EN 1 PERO VA EN < 2
        {
            waitImage.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            waitImage.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void AddToList(NetworkPlayer player)
    {
        var playerRef = player.Object.StateAuthority;

        if (_players.Contains(playerRef))
            return;

        _players.Add(playerRef);
    }

    void RemoveFromList(PlayerRef player)
    {
        _players.Remove(player);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Defeat(PlayerRef defeatedPlayer)
    {
        Debug.Log($"Player {defeatedPlayer} fue derrotado. Yo soy {Runner.LocalPlayer}");

        if (Runner.LocalPlayer == defeatedPlayer)
        {
            Debug.Log("Mostrando DERROTA");
            Defeat();
        }
        else
        {
            Debug.Log("Mostrando VICTORIA");
            Win();
        }

        RemoveFromList(defeatedPlayer);
    }

    //[RpcTarget] El llamado del RPC va a ir dirigido a ese jugador
    [Rpc]
    void RPC_Win([RpcTarget] PlayerRef player)
    {
        Win();
    }

    void Win()
    {
        _winImage.SetActive(true);
    }

    void Defeat()
    {
        _loseImage.SetActive(true);
    }

}
