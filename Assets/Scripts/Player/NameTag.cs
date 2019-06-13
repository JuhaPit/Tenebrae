using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTag : MonoBehaviour {

	public Transform rayCastPoint;
	RaycastHit hit;
	Transform lastPlayerAimed;

	void Update() {
		Vector3 forward = rayCastPoint.TransformDirection(Vector3.forward);
		if (Physics.Raycast(rayCastPoint.position, forward, out hit)) {
			if (hit.transform.CompareTag("Player") && lastPlayerAimed == null) {
				lastPlayerAimed = hit.transform;
				lastPlayerAimed.GetComponent<Player>().nameTag.gameObject.SetActive(true);
			} else {
				if (lastPlayerAimed != null && hit.transform != lastPlayerAimed) {
					lastPlayerAimed.GetComponent<Player>().nameTag.gameObject.SetActive(false);
					lastPlayerAimed = null;
				}
			}
		}
	}
}
