using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TypingBox : MonoBehaviour
{
    public static event Action<string> OnWordUpdate;
    public static event Action<string> OnWordSubmission;

    [SerializeField] TextMeshProUGUI m_Text;

    void Awake()
    {
        m_Text.text = "";
    }

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                if (m_Text.text.Length > 0)
                {
                    m_Text.text = m_Text.text.Remove(m_Text.text.Length - 1);
                }
            }
            else if (c == ' ' ||
                     c == '\n' ||
                     c == '\r')
            {
                OnWordSubmission?.Invoke(m_Text.text);
                m_Text.text = "";
            }
            else
            {
                m_Text.text += c;
            }

            OnWordUpdate?.Invoke(m_Text.text);
        }
    }
}
