using UnityEngine;

public class ViewInitializer : MonoBehaviour {

	public ViewBase entryView;
	
	void Awake() {
		ViewBase[] views = GameObject.FindObjectsOfType<ViewBase>();

		foreach(ViewBase view in views) {
			view.viewObject.SetActive(false);
		}
		entryView.viewObject.SetActive(true);
	}
}
