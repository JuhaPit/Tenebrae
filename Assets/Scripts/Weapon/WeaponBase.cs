using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponBase : MonoBehaviour {

	protected Animator animator;
	protected AudioSource audioSource;
	protected FirstPersonController controller;
	[HideInInspector] public bool canFire = false;
	protected FireMode fireMode;
	[HideInInspector] public bool isReloading = false;

	string impactPrefabName = "BulletImpactEffect";
	string bloodPrefabName = "BloodEffect";

	[Header("Object References")]
	public ParticleSystem muzzleFlash;
	public Transform rayCastPoint;
	public Transform crossHair;

	[Header("UI References")]
	public Text weaponName;
	public Text ammo;
	public string displayedName;

	[Header("Sound References")]
	public AudioClip drawSound;
	public AudioClip fireSound;
	public AudioClip dryFireSound;
	public AudioClip magEjectSound;
	public AudioClip magInsertSound;
	public AudioClip boltCatchSound;

	[Header("Attributes")]
	public float damage = 20f;
	public float roundsPerMinute = 400f;
	protected int bulletsInMag;
	public int magazineSize = 12;
	public int maxAmmo = 100;
	protected int ammoLeft;
	public float accuracy = 0.7f;
	public float recoil = 1.0f;

	// Use this for initialization

	void Awake() {
		weaponName = GameObject.Find("WeaponName").GetComponent<Text>();
		ammo = GameObject.Find("Ammo").GetComponent<Text>();
		crossHair = GameObject.Find("CrossHair").transform;
	}

	void Start () {
		controller = GameObject.FindObjectOfType<FirstPersonController>();
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		SetFireMode();
		bulletsInMag = magazineSize;
		ammoLeft = maxAmmo;
		UpdateTexts();
	}

	public void UpdateTexts() {
		weaponName.text = displayedName;
		ammo.text = bulletsInMag + " / " + ammoLeft;
	}
	
	// Update is called once per frame
	void Update () {
		listenFire();
		listenReload();
	}

	void listenFire() {
		if (WeaponFired()) {
			if (bulletsInMag > 0) {
				Fire();
			} else {
				DryFire();
			}
		}
	}

	void listenReload() {
		if(WeaponReloaded()) {
			Player.instance.PlayReloadAnimation();
			Reload();
		}
	}

	protected virtual bool WeaponFired() {
		return Input.GetButtonDown("Fire1") && canFire && !isReloading;
	}

	bool WeaponReloaded() {
		return Input.GetButtonDown("Reload") && bulletsInMag < magazineSize && ammoLeft > 0 && !isReloading && canFire;
	}

	void Fire() {
		muzzleFlash.Stop();
		muzzleFlash.Play();
		Player.instance.PlayMuzzleFlashThroughPhoton(displayedName);
		audioSource.PlayOneShot(fireSound);
		Player.instance.PlaySoundThroughPhoton(fireSound.name);
		canFire = false;
		Player.instance.PlayFireAnimation();
		PlayFireAnimation();
		DoHitScan();
		Recoil();
		bulletsInMag--;
		UpdateTexts();
		StartCoroutine(ResetFireAbility());
	}

	public virtual void PlayFireAnimation() {
		animator.CrossFadeInFixedTime("Fire", 0.1f);
	}

	void Recoil() {
		controller.mouseLook.SetRecoil(recoil);
	}

	protected virtual void DoHitScan() {
		HitScan();
	}

	void HitScan() {
		RaycastHit hit;
		if (Physics.Raycast(rayCastPoint.position, CalculateSpread(accuracy, rayCastPoint), out hit)) {
			string effect;
			if (hit.transform.CompareTag("Player")) {
				effect = bloodPrefabName;
				PhotonView photonPlayer = hit.transform.GetComponent<PhotonView>();
				photonPlayer.RPC("RPCDamage", PhotonTargets.AllBuffered, damage);
			} else {
				effect = impactPrefabName;
			}
			Debug.Log(hit.transform.name);
			GameObject impact = PhotonNetwork.Instantiate(effect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal), 0);
			DestroyAfterTime(impact, 1f);
		}
	}

	void DestroyAfterTime(GameObject obj, float time) {
		StartCoroutine(CoDestroyAfterTime(obj, time));
	}

	IEnumerator CoDestroyAfterTime(GameObject obj, float time) {
		yield return new WaitForSeconds(time);
		PhotonNetwork.Destroy(obj);
	}

	Vector3 CalculateSpread(float accuracy, Transform rayCastPoint) {
		return Vector3.Lerp(rayCastPoint.TransformDirection(Vector3.forward * 100), Random.onUnitSphere, accuracy);
	}

	protected virtual void DryFire() {
		audioSource.PlayOneShot(dryFireSound);
		Player.instance.PlaySoundThroughPhoton(dryFireSound.name);
		canFire = false;
		StartCoroutine(ResetFireAbility());
	}

	protected virtual void Reload() {
		isReloading = true;
		animator.CrossFadeInFixedTime("Reload", 0.1f);
	}

	protected virtual void ReloadAmmo() {
		int bulletsMissingFromMagazine = magazineSize - bulletsInMag;
		int bulletsForReload = (ammoLeft >= bulletsMissingFromMagazine) ? bulletsMissingFromMagazine : ammoLeft;
		ammoLeft -= bulletsForReload;
		bulletsInMag += bulletsForReload;
		UpdateTexts();
	}

	IEnumerator ResetFireAbility() {
		yield return new WaitForSeconds(60 / roundsPerMinute);
		canFire = true;
	}

	public virtual void OnDraw() {
		audioSource.PlayOneShot(drawSound);
		Player.instance.PlaySoundThroughPhoton(drawSound.name);
	}

	public void WeaponDrawn() {
		canFire = true;
	}

	public void OnMagEject() {
		audioSource.PlayOneShot(magEjectSound);
		Player.instance.PlaySoundThroughPhoton(magEjectSound.name);
	}

	public virtual void OnMagInsert() {
		audioSource.PlayOneShot(magInsertSound);
		Player.instance.PlaySoundThroughPhoton(magInsertSound.name);
		ReloadAmmo();
	}

	public virtual void OnBoltCatch() {
		audioSource.PlayOneShot(boltCatchSound);
		Player.instance.PlaySoundThroughPhoton(boltCatchSound.name);
	}

	public void ReloadFinished() {
		isReloading = false;
	}

	protected virtual void SetFireMode() {
		fireMode = FireMode.SemiAuto;
	}
}
