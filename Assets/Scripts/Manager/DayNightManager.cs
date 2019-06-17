using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class DayNightManager : Photon.PunBehaviour {
 
    public Light sun;
    public float secondsInFullDay = 120f;
    [Range(0,1)]
    public float currentTimeOfDay = 0;
    [HideInInspector]
    public float timeMultiplier = 1f;
    float sunInitialIntensity;
    public Text timeText;

    void Start() {
        sunInitialIntensity = sun.intensity;
    }
    
    void Update() {
		if(PhotonNetwork.isMasterClient) {
			photonView.RPC("UpdateSun", PhotonTargets.AllBuffered, currentTimeOfDay);
			currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;
			if (currentTimeOfDay >= 1) {
				currentTimeOfDay = 0;
			}
		}
    }
    
	[PunRPC]
    void UpdateSun(float time) {
        sun.transform.localRotation = Quaternion.Euler((time * 360f) - 90, 170, 0);
 
        float intensityMultiplier = 1;
        if (time <= 0.23f || time >= 0.75f) {
            intensityMultiplier = 0;
        }
        else if (time <= 0.25f) {
            intensityMultiplier = Mathf.Clamp01((time - 0.23f) * (1 / 0.02f));
        }
        else if (time >= 0.73f) {
            intensityMultiplier = Mathf.Clamp01(1 - ((time - 0.73f) * (1 / 0.02f)));
        }
 
        sun.intensity = sunInitialIntensity * intensityMultiplier;
        UpdateTimeForUI(time);
    }

    void UpdateTimeForUI(float time) {
        float currentHour = 24 * time;
        float currentMinute = 60 * (currentHour - Mathf.Floor(currentHour));
        timeText.text = formatTime(currentHour) + ":" + formatTime(currentMinute);
    }

    string formatTime(float time) {
        time = Mathf.Floor(time);
        if(time < 10f) {
            return "0" + time;
        } else {
            return time.ToString();
        }
    }
}
