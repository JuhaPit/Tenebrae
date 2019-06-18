using UnityEngine;
using System.Collections;

public class MouseLock : MonoBehaviour {

  public bool Enable = false;

	void OnApplicationFocus(bool status)
	{
		if (Enable && status)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	} 
  
}
