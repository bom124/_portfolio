using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DropDownMicrofon : MonoBehaviour
{
  Dropdown m_Dropdown;
  string[] devices = { "Non" };
  void Start()
  {
    m_Dropdown = GetComponent<Dropdown>();
    m_Dropdown.ClearOptions();
  }
  void Update()
  {
    if (!devices.SequenceEqual(Microphone.devices))
    {
      m_Dropdown.ClearOptions();
      devices = Microphone.devices;
      foreach (string item in devices) 
      {
        Dropdown.OptionData option = new Dropdown.OptionData() { text = item };
        m_Dropdown.options.Add(option);
      }
    }
    Options.device_name = devices[m_Dropdown.value];
    m_Dropdown.captionText.text = Options.device_name;
    }
}
