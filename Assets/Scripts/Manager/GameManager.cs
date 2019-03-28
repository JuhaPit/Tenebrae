using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Photon.PunBehaviour {

	public static GameManager instance;
	public GameObject playerPrefab;
	public Transform playerSpawn;

	void Awake() {
		instance = this;
	}

	public void StartGame() {
		PhotonNetwork.Instantiate("Player", playerSpawn.position, playerSpawn.rotation, 0);
	}

	
}
