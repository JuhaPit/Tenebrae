using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GameManager : Photon.PunBehaviour
{

    public static GameManager instance;
    public GameObject playerPrefab;
    public Transform playerSpawn;
    List<string> possibleRoles;

    void Awake()
    {
        instance = this;
        possibleRoles = new List<string> {"Traitor"};
    }

    public void StartGame()
    {
        GameObject player = PhotonNetwork.Instantiate("Player", playerSpawn.position, playerSpawn.rotation, 0);
        AssignRole(player);
	}

    public void AssignRole(GameObject player) {
		string role = possibleRoles[Random.Range(0, possibleRoles.Count)];
        Type roleType = Type.GetType(role);
		possibleRoles.Remove(role);
		player.AddComponent(roleType);
	}
}
