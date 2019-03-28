using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour {

	private float amount = 0.1f;
	private float swayRange = 0.3f;
	private float smooth = 6.0f;

	private Vector3 initialPosition;

	// Use this for initialization
	void Start () {
		initialPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		float moveX = -Input.GetAxis("Mouse X") * amount;
		float moveY = -Input.GetAxis("Mouse Y") * amount;
		moveX = Mathf.Clamp(moveX, -swayRange, swayRange);
		moveY = Mathf.Clamp(moveY, -swayRange, swayRange);
		Vector3 finalPosition = new Vector3(moveX, moveY, 0);
		transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smooth);
	}
}
