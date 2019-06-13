using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class BaseRole : MonoBehaviour {

	[HideInInspector]
	public abstract string roleName {get;}
	[HideInInspector]
	public abstract string roleDescription {get;}
	[HideInInspector]
	public Text roleNameField;
	[HideInInspector]
	public Text roleDescriptionField;
	[HideInInspector]
	public GameObject rolePanel;

	void Awake() {
		rolePanel = GameObject.Find("/Canvas/InGame/RolePanel");
		roleNameField = rolePanel.transform.Find("RoleName").GetComponent<Text>();
		roleDescriptionField = rolePanel.transform.Find("RoleDescription").GetComponent<Text>();
	}

	void Start() {
		roleNameField.text = roleName;
		roleDescriptionField.text = roleDescription;
	}

	void Update() {
		showRolePanel(Input.GetKey(KeyCode.Tab));
	}

	void showRolePanel(bool visible) {
		rolePanel.SetActive(visible);
	}


}
