using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Addons.Physics;

public class PlayerSpawning : SimulationBehaviour, IPlayerJoined
{
    public GameObject playerPrefab;

    private int playersCount;

    [SerializeField] private Transform[] _spawnTransforms;

    //private bool _initialized;

    //Se ejecuta por CADA cliente conectado
    public void PlayerJoined(PlayerRef player)
    {
        playersCount = Runner.SessionInfo.PlayerCount;

        Debug.Log("Jugador conectado. Total: " + playersCount);

        // Solo el Host (Server) controla el spawn
        if (Runner.IsSharedModeMasterClient && playersCount == 2)
        {
            Debug.Log("¡Hay 2 jugadores, spawneando!");

            // Spawnea a ambos jugadores
            SpawnAllPlayers();
        }
    }

    private void SpawnAllPlayers()
    {
        for (int i = 0; i < playersCount; i++)
        {
            CreatePlayer(i);
        }
    }

    void CreatePlayer(int spawnPointIndex)
    {
        //_initialized = false;

        var newPosition = _spawnTransforms[spawnPointIndex].position;
        var newRotation = _spawnTransforms[spawnPointIndex].rotation;

        //Instancio en red (para todos) a mi personaje
        Runner.Spawn(playerPrefab, newPosition, newRotation);
    }
}
