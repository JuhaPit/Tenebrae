using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : Photon.PunBehaviour {
	
	public static Player instance;
	public Animator characterAnimator;
	private Vector3 syncPos = Vector3.zero;
	private Quaternion syncRot = Quaternion.identity;
	private FirstPersonController fpsController;
	public Transform characterWeapons;
	public MuzzleFlashController muzzleFlashController;

	void Awake() {
		if(photonView.isMine) {
			instance = this;
		}
		syncPos = transform.position;
		syncRot = transform.rotation;
	}

	void Start() {
		muzzleFlashController = GetComponent<MuzzleFlashController>();
		if (!photonView.isMine) {
			Destroy(transform.Find("FirstPersonCharacter").gameObject);
			transform.Find("PlayerCharacter").gameObject.SetActive(true);
			MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
			foreach(MonoBehaviour script in scripts) {
				if (script == this || script is PhotonView || script is WeaponADS) {
					continue;
				}
				Destroy(script);
			}
			return;
		} else {
			Destroy(transform.Find("PlayerCharacter").gameObject);
		}
		fpsController = GetComponent<FirstPersonController>();
	}

	void Update() {
		if(!photonView.isMine) {
			transform.position = Vector3.Lerp(transform.position, syncPos, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, syncRot, 0.1f);
			return;
		}
		UpdateAnimator();
	}

	public void PlaySoundThroughPhoton(string clipName) {
		photonView.RPC("RPCPlaySoundThroughPhoton", PhotonTargets.Others, clipName);
	}

	[PunRPC]
	void RPCPlaySoundThroughPhoton(string clipName) {
		AudioClip clip = (AudioClip)Resources.Load("Audios/" + clipName, typeof(AudioClip));
		AudioSource.PlayClipAtPoint(clip, gameObject.transform.position);
	}

	public void PlayMuzzleFlashThroughPhoton(string identifier) {
		photonView.RPC("RPCPlayMuzzleFlashThroughPhoton", PhotonTargets.Others, identifier);
	}

	[PunRPC]
	void RPCPlayMuzzleFlashThroughPhoton(string identifier) {
		muzzleFlashController.PlayMuzzleFlash(identifier);
	}

	public void SetWeapon(string weaponName) {
		photonView.RPC("RPCSetWeapon", PhotonTargets.Others, weaponName);
	}

	void DisableAllEquippedWeapons() {
		characterAnimator.SetBool("IsPolice9mm", false);
		characterAnimator.SetBool("IsCompact9mm", false);
		characterAnimator.SetBool("IsDefenderShotgun", false);
		characterAnimator.SetBool("IsPortableMagnum", false);
		characterAnimator.SetBool("IsStovRifle", false);
		characterAnimator.SetBool("IsUMP45", false);
		for(int i = 0; i < characterWeapons.childCount; i++) {
			characterWeapons.GetChild(i).gameObject.SetActive(false);
		}
	}

	[PunRPC]
	void RPCSetWeapon(string weaponName) {
		characterAnimator.SetTrigger("SwitchWeapon");
		DisableAllEquippedWeapons();
		characterAnimator.SetBool("Is" + weaponName, true);
		characterWeapons.Find(weaponName).gameObject.SetActive(true);
	}

	public void PlayFireAnimation() {
		photonView.RPC("RPCSetAnimationTrigger", PhotonTargets.Others, "Firing");
	}

	public void PlayReloadAnimation() {
		photonView.RPC("RPCSetAnimationTrigger", PhotonTargets.Others, "Reloading");
	}

	[PunRPC]
	void RPCSetAnimationTrigger(string trigger) {
		characterAnimator.SetTrigger(trigger);
	}

	void UpdateAnimator() {
		photonView.RPC("RPCSyncAnimator", PhotonTargets.Others, fpsController.controller.velocity.magnitude);
	}

	[PunRPC]
	void RPCSyncAnimator(float speed) {
		if(speed <= 0) {
			characterAnimator.SetBool("isWalking", false);
			characterAnimator.SetBool("isRunning", false);
		} else if(speed <= 5.5f) {
			characterAnimator.SetBool("isRunning", false);
			characterAnimator.SetBool("isWalking", true);
		} else {
			characterAnimator.SetBool("isWalking", false);
			characterAnimator.SetBool("isRunning", true);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if(stream.isWriting) {
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		} else {
			syncPos = (Vector3) stream.ReceiveNext();
			syncRot = (Quaternion) stream.ReceiveNext();
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
		SetWeapon(WeaponSwitch.instance.GetCurrentWeapon());
	}
}
