using UnityEngine;
using UnityEngine.UI;

public class OptionsView : ViewBase {

	[Header("View References")]
	public ViewBase startView;
	public Button backButton;

	protected override void OnInit() {
		backButton.onClick.AddListener(() => {
			this.Hide();
			startView.Show();
		});
	}
}
