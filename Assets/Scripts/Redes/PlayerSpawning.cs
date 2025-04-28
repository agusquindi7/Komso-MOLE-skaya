using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Addons.Physics;

public class PlayerSpawning : SimulationBehaviour, IPlayerJoined
{
    public GameObject playerPrefab;

    [SerializeField] private Transform[] _spawnTransforms;

    private bool _initialized;

    //Se ejecuta por CADA cliente conectado
    public void PlayerJoined(PlayerRef player)
    {
        var playersCount = Runner.SessionInfo.PlayerCount;

        if (_initialized && playersCount >= 2)
        {
            CreatePlayer(0);
            return;
        }

        //Si el cliente que entro, es el mismo cliente donde corre este codigo, entonces:
        if (player == Runner.LocalPlayer)
        {
            if (playersCount < 2)
                _initialized = true;
            else
            {
                CreatePlayer(playersCount - 1);
            }
        }
    }

    void CreatePlayer(int spawnPointIndex)
    {
        _initialized = false;

        var newPosition = _spawnTransforms[spawnPointIndex].position;
        var newRotation = _spawnTransforms[spawnPointIndex].rotation;

        //Instancio en red (para todos) a mi personaje
        Runner.Spawn(playerPrefab, newPosition, newRotation);
    }
}
