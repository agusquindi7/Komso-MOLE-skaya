using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject _winImage;
    [SerializeField] private GameObject _loseImage;
    private List<PlayerRef> _players = new List <PlayerRef>();
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void AddToList(Player player)
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

    [Rpc]
    public void RPC_Defeat(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Win();
        }

        RemoveFromList(player);

        if (_players.Count == 1 && HasStateAuthority)
            RPC_Win(_players[0]);
    }

    //[RpcTarget] El llamado del RPC va a ir dirigido a ese jugador
    [Rpc]
    void RPC_Win([RpcTarget] PlayerRef player)
    {
        Defeat();
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
