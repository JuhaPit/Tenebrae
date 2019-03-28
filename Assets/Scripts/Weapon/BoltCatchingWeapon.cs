using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltCatchingWeapon : WeaponBase {

	public override void PlayFireAnimation() {
		string fireAnimation = bulletsInMag > 1 ? "Fire" : "FireLast";
		animator.CrossFadeInFixedTime(fireAnimation, 0.1f);
	}
}
