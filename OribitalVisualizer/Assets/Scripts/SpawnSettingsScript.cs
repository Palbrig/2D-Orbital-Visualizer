using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnSettingsScript : MonoBehaviour
{
  [Header("UI References")]
  public TMP_InputField massInp;
  public TMP_InputField velScaleInp;

  [Header("Default Values")]
  public float defaultMass = 5f;
  public float defaultVelocity = 1f;

  public float GetMass()
  {
    try
    {
      if (float.TryParse(massInp.text, out float mass))
      {
        return Mathf.Max(0.1f, mass);
      }
      else { return defaultMass; }
    }
    catch (Exception e)
    {
      return defaultMass;
    }
  }

  public float GetVelocityScale()
  {
    try
    {
      if (float.TryParse(velScaleInp.text, out float vel))
        return Mathf.Max(0f, vel);
      return defaultVelocity;
    }
    catch (Exception e) 
    { 
      return defaultVelocity;
    }
  }

  private void Start()
  {
     massInp.text = defaultMass.ToString();
    velScaleInp.text = defaultVelocity.ToString();
  }
}
