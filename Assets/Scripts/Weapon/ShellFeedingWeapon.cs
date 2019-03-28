using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellFeedingWeapon : WeaponBase {

	private int pellets = 8;

	protected override void DoHitScan() {
		for(int i = 0; i < pellets; i++) {
			base.DoHitScan();
		}
	}

	protected override void Reload() {
		isReloading = true;
		string animationName = bulletsInMag == 0 ? "ReloadStartEmpty" : "ReloadStart";
		animator.CrossFadeInFixedTime(animationName, 0.1f);
	}

	protected override void ReloadAmmo() {
		ammoLeft--;
		bulletsInMag++;
		base.UpdateTexts();
	}

	void CheckNextReload() {
		string animationName; 
		if (ammoLeft == 0 || bulletsInMag == magazineSize) {
			animationName = "ReloadEnd";
		} else {
			animationName = "ReloadInsert";
		}
		animator.CrossFadeInFixedTime(animationName, 0.1f);
	}

	public void OnPump() {
		audioSource.PlayOneShot(boltCatchSound);
	}
}
