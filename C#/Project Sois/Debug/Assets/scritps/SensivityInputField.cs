using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SensivityInputField : MonoBehaviour
{
    InputField m_InputField;
    void Start()
    {
        m_InputField= GetComponent<InputField>();
    }
    public void InputSensivity()
    {
        int.TryParse(m_InputField.text, out Options.sensivity_global);
    }
}
