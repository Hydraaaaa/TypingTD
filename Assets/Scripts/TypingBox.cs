using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TypingBox : MonoBehaviour
{
    public static event Action<string> OnWordUpdate;
    public static event Action<string> OnWordSubmission;

    [SerializeField] TMP_InputField m_InputField;

    void Awake()
    {
        OnDeselected();
    }

    void Update()
    {
        if (m_InputField.isFocused)
        {
            m_InputField.caretPosition = m_InputField.text.Length;
            m_InputField.text = m_InputField.text.ToLower();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            OnSubmitted();

            m_InputField.text = "";
        }
    }

    public void OnValueChanged()
    {
        OnWordUpdate?.Invoke(m_InputField.text);
    }

    void OnSubmitted()
    {
        OnWordSubmission?.Invoke(m_InputField.text);
    }

    public void OnDeselected()
    {
        m_InputField.Select();
        m_InputField.ActivateInputField();
    }
}
