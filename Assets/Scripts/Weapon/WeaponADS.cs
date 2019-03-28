using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponADS : MonoBehaviour {

	private FirstPersonController fpsController;
	private WeaponBob weaponBob;
	private Vector3 originalPosition;
	private float originalWalkSpeed;
	private float originalRunSpeed;
	private float originalBobbingAmount;
	[Header("Attributes")]
	public Vector3 aimPosition;
	public float adsSpeed = 15;
	private WeaponBase weaponBase;
	private float initialAccuracy;

	// Use this for initialization
	void Start () {
		fpsController = gameObject.transform.parent.parent.parent.parent.transform.GetComponent<FirstPersonController>();
		weaponBob = gameObject.transform.parent.gameObject.GetComponent<WeaponBob>();
		originalBobbingAmount = weaponBob.bobbingAmount;
		weaponBase = GameObject.FindObjectOfType<WeaponBase>();
		initialAccuracy = weaponBase.accuracy;
		originalWalkSpeed = fpsController.walkSpeed;
		originalRunSpeed = fpsController.runSpeed;
		originalPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		AimDownSights();
	}

	void AimDownSights() {
		if (AimDownAttempted()) {
			HandleAimDownSights();
		} else {
			ResetNormalAimStance();
		}
	}

	void MoveTo(Vector3 position) {
		transform.localPosition = Vector3.Lerp(transform.localPosition, position, Time.deltaTime * adsSpeed);
	}

	bool AimDownAttempted() {
		return Input.GetButton("Fire2") && !weaponBase.isReloading;
	}

	void HandleAimDownSights() {
		MoveTo(aimPosition);
		SetAccuracy(0f);
		SetCrossHairVisible(false);
		fpsController.walkSpeed = originalWalkSpeed / 2;
		fpsController.runSpeed = fpsController.walkSpeed;
		weaponBob.bobbingAmount = originalBobbingAmount / 10;
	}

	public void ResetNormalAimStance() {
		MoveTo(originalPosition);
		SetAccuracy(initialAccuracy);
		SetCrossHairVisible(true);
		fpsController.walkSpeed = originalWalkSpeed;
		fpsController.runSpeed = originalRunSpeed;
		weaponBob.bobbingAmount = originalBobbingAmount;
	}

	void SetAccuracy(float value) {
		weaponBase.accuracy = value;
	}

	void SetCrossHairVisible(bool selection) {
		weaponBase.crossHair.gameObject.SetActive(selection);
	}
}
