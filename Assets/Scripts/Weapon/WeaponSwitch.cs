using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponSwitch : MonoBehaviour {

	public static WeaponSwitch instance;
	public int currentWeaponIndex = 0;
	private Weapon[] weapons = { Weapon.Police9mm, Weapon.PortableMagnum, Weapon.Compact9mm, Weapon.StovRifle, Weapon.UMP45, Weapon.DefenderShotgun };
	public Transform weaponManager;
	private GameObject weapon;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	void Start() {
		SetCurrentWeaponActive(true);
	}

	void SetCurrentWeaponActive(bool selection) {
		weapon = weaponManager.Find(weapons[currentWeaponIndex].ToString()).gameObject;
		if (!selection) {
			ResetWeaponParameters(weapon);
		}
		weapon.SetActive(selection);
		Player.instance.SetWeapon(weapon.name);
		weapon.GetComponent<WeaponBase>().UpdateTexts();
	}

	public string GetCurrentWeapon() {
		return weapon.name;
	}

	void Update() {
		CheckWeaponSwitch();
	}

	void CheckWeaponSwitch() {
		float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

		if (mouseWheel > 0) {
			selectPreviousWeapon();
		} else if (mouseWheel < 0) {
			selectNextWeapon();
		}
	}

	void selectPreviousWeapon() {
		SetCurrentWeaponActive(false);
		if(currentWeaponIndex == 0) {
			currentWeaponIndex = weapons.Length -1;
		} else {
			currentWeaponIndex--;
		}
		SetCurrentWeaponActive(true);
	}

	void selectNextWeapon() {
		SetCurrentWeaponActive(false);
		if(currentWeaponIndex == weapons.Length -1) {
			currentWeaponIndex = 0;
		} else {
			currentWeaponIndex++;
		}
		SetCurrentWeaponActive(true);
	}

	void ResetWeaponParameters(GameObject weapon) {
		WeaponADS adsComponent = weapon.GetComponent<WeaponADS>();
		if (adsComponent != null) {
			adsComponent.ResetNormalAimStance();
		}
		WeaponBase weaponBase = weapon.GetComponent<WeaponBase>();
		weaponBase.canFire = false;
		weaponBase.isReloading = false;
	}
}
