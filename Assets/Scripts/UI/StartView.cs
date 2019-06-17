using UnityEngine;
using UnityEngine.UI;
using System;

public class StartView : ViewBase {

	[Header("View References")]
	public ViewBase optionsView;
	public GameObject lobbyCamera;
	public GameObject inGameUI;
	public GameObject mainUI;
	public InputField nameField;
	public Button startButton;
	public Button optionsButton;
	public Button exitButton;

	protected override void OnInit() {
		nameField.text = PlayerPrefs.GetString("Player_Name");
		startButton.onClick.AddListener(() => {
			if (String.IsNullOrEmpty(nameField.text)) {
				return;
			}
			PlayerPrefs.SetString("Player_Name", nameField.text);
			NetworkManager.instance.Connect(
				() => {
					NetworkManager.instance.JoinOrCreateRoom(
						() => {
							mainUI.SetActive(false);
							lobbyCamera.SetActive(false);
							inGameUI.SetActive(true);
							GameManager.instance.StartGame();
						}
					);
				},
				() => {
					print("Failed to connect");
				}
			);
		});

		optionsButton.onClick.AddListener(() => {
			this.Hide();
			optionsView.Show();
		});

		exitButton.onClick.AddListener(() => {
			Application.Quit();
		});
	}
}
