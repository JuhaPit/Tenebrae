using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spectator : MonoBehaviour {

	private bool spectateEnabled = false;
	GameObject[] players;
	int playerIndex = -1;
	GameObject spectatedCamera;
	GameObject deathPanel;
	GameObject spectatorPanel;

	void Awake() {
		deathPanel = GameObject.Find("/Canvas/InGame/DeathPanel");
		spectatorPanel = GameObject.Find("/Canvas/InGame/SpectatorPanel");
	}
	void Update () {
		if (spectateEnabled) {
			handleSpectate();
		}
	}

	void handleSpectate() {
		if(Input.GetKeyDown("space")) {
			deathPanel.SetActive(false);
			spectatorPanel.SetActive(true);
			spectateNextPlayer(getPlayerIndex());
		}
	}

	void spectateNextPlayer(int index) {
		GameObject playerCam = players[index].transform.Find("DeathCam").gameObject;
		spectatedCamera.SetActive(false);
		playerCam.SetActive(true);
		spectatedCamera = playerCam;
		spectatorPanel.GetComponentInChildren<Text>().text = GetPlayerName(players[index]);
	}

	public void enableSpectate(GameObject deathCam) {
		spectatedCamera = deathCam;
		players = GameObject.FindGameObjectsWithTag("PlayerModel");
		spectateEnabled = players.Length > 0;
	}

	int getPlayerIndex() {
		if((playerIndex + 1) > (players.Length - 1)) {
			playerIndex = 0;
		} else {
			playerIndex++;
		}
		return playerIndex;
	}

	string GetPlayerName(GameObject player) {
		TextMesh nameFromPlayer = player.GetComponent<TextMesh>();
		if(nameFromPlayer != null) {
			return nameFromPlayer.text;
		} else {
			return "test";
		}
	}
}
