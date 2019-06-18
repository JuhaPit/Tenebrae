using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRWorksState : MonoBehaviour {

  private NVIDIA.VRWorks m_VRWorks = null;

  // Use this for initialization
  void Start ()
  {
    m_VRWorks = GetComponent<Camera>().GetComponent<NVIDIA.VRWorks>();
	}

  private GUIStyle style = new GUIStyle();
  void Awake()
  {
    style.normal.textColor = new Color(1, 1, 1);
    style.fontSize = 14;
    style.fontStyle = FontStyle.Bold;
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyUp(KeyCode.Return))
    {
      Time.timeScale = Time.timeScale > 0.0f ? 0.0f : 1.0f;
    }
    else if (Input.GetKeyUp(KeyCode.M))
    {
      if (m_VRWorks.IsFeatureAvailable(NVIDIA.VRWorks.Feature.MultiResolution))
      {
        if (m_VRWorks.GetActiveFeature() != NVIDIA.VRWorks.Feature.MultiResolution)
        {
          m_VRWorks.SetActiveFeature(NVIDIA.VRWorks.Feature.MultiResolution);
        }
        else
        {
          m_VRWorks.SetActiveFeature(NVIDIA.VRWorks.Feature.None);
        }
      }
    }
  }

  void OnGUI()
  {
    if(Camera.current != null)
    {
      NVIDIA.VRWorks.FeatureData data = m_VRWorks.GetActiveFeatureData();
      float ratio = 100.0f;
      string mode = "Off";
      if (m_VRWorks.GetActiveFeature() != NVIDIA.VRWorks.Feature.None)
      {
        mode = "On";
        ratio = 100.0f * ((data.boundingRect[2] - data.boundingRect[0]) * (data.boundingRect[3] - data.boundingRect[1]) / (Camera.current.pixelWidth * Camera.current.pixelHeight));
      }
      mode += " (" + ratio.ToString("F2") + "% pixels rendered)";
      GUI.Label(new Rect(10, 10, 300, 30), "MRS: " + mode, style);
    }    
  }
}
