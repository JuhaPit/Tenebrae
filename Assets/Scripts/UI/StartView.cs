using UnityEngine;
using UnityEngine.UI;

public class StartView : ViewBase {

	[Header("View References")]
	public ViewBase optionsView;
	public GameObject lobbyCamera;
	public GameObject inGameUI;
	public GameObject mainUI;
	public Button startButton;
	public Button optionsButton;
	public Button exitButton;

	protected override void OnInit() {
		startButton.onClick.AddListener(() => {
			NetworkManager.instance.Connect(
				() => {
					mainUI.SetActive(false);
					NetworkManager.instance.JoinOrCreateRoom(
						() => {
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
