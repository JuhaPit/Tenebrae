using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }
    Slider healthBar;
    public Animator hitAnimator;
    public Animator deathAnimator;

	void Awake() {
		GameObject healthPanel = GameObject.Find("HealthPanel");
		healthBar = healthPanel.GetComponentInChildren<Slider>();
        hitAnimator = GameObject.Find("/Canvas/InGame/Hit").GetComponent<Animator>();
        deathAnimator = GameObject.Find("/Canvas/InGame/DeathPanel").GetComponent<Animator>();
	}

    void Start () {
        MaxHealth = 100f;
        CurrentHealth = MaxHealth;
        healthBar.value = CalculateHealth();
	}

    public void DealDamage(float damageValue) {
        CurrentHealth -= damageValue;
        healthBar.value = CalculateHealth();
        hitAnimator.CrossFadeInFixedTime("Hit", 0.1f);
        if (CurrentHealth <= 0) {
            Die();
        }
    }

    float CalculateHealth() {
        return CurrentHealth / MaxHealth;
    }

    void Die() {
        CurrentHealth = 0;
        deathAnimator.SetTrigger("Show");
        Player.instance.GetComponent<PhotonView>().RPC("RPCKillPlayer", PhotonTargets.All);
    }
}
