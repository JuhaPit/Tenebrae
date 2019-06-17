using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GameManager : Photon.PunBehaviour
{

    public static GameManager instance;
    public GameObject playerPrefab;
    List<string> possibleRoles;
    GameObject[] possibleSpawns;


    void Awake()
    {
        instance = this;
        possibleRoles = new List<string> {"Traitor"};
        possibleSpawns = GameObject.FindGameObjectsWithTag("Spawn");
    }

    public void StartGame()
    {
        GameObject player = SpawnPlayer();
        AssignRole(player);
	}

    GameObject SpawnPlayer() {
        Transform spawn = possibleSpawns[Random.Range(0, possibleSpawns.Length)].transform;
        return PhotonNetwork.Instantiate("Player", spawn.position, spawn.rotation, 0);
    }

    void AssignRole(GameObject player) {
		string role = possibleRoles[Random.Range(0, possibleRoles.Count)];
        Type roleType = Type.GetType(role);
		possibleRoles.Remove(role);
		player.AddComponent(roleType);
	}
}
