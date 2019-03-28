using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashController : MonoBehaviour {

	public ParticleSystem police9mmFlash;
	public ParticleSystem compact9mmFlash;
	public ParticleSystem portableMagnumFlash;
	public ParticleSystem stovRifleFlash;
	public ParticleSystem UMP45Flash;
	public ParticleSystem defenderShotgunFlash;

	public void PlayMuzzleFlash(string identifier) {
		switch(identifier) {
			case "Glock 18C":
				init(police9mmFlash);
				break;
			case "MP5K":
				init(compact9mmFlash);
				break;
			case "Colt Python":
				init(portableMagnumFlash);
				break;
			case "AK-47":
				init(stovRifleFlash);
				break;
			case "UMP45":
				init(UMP45Flash);
				break;
			case "M870":
				init(defenderShotgunFlash);
				break;
		}
	}

	void init(ParticleSystem flash) {
		flash.Stop();
		flash.Play();
	}
}
