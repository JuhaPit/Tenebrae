using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : WeaponBase {

	protected override bool WeaponFired() {
		return (fireMode == FireMode.SemiAuto ? Input.GetButtonDown("Fire1") : Input.GetButton("Fire1")) && canFire && !isReloading;
	}

	protected override void DryFire() {
		fireMode = FireMode.SemiAuto;
		base.DryFire();
	}

	protected override void Reload() {
		fireMode = FireMode.FullAuto;
		base.Reload();
	}

	protected override void SetFireMode() {
		fireMode = FireMode.FullAuto;
	}
}
