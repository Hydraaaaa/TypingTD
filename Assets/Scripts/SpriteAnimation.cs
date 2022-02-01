using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] float m_Interval = 0.2f;
    [SerializeField] Sprite[] m_Sprites;

    float m_CurrentInterval = 0.0f;
    int m_Index = 0;

    void Reset()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        m_CurrentInterval += Time.deltaTime;

        if (m_CurrentInterval >= m_Interval)
        {
            m_CurrentInterval -= m_Interval;

            m_Index++;

            if (m_Index >= m_Sprites.Length)
            {
                m_Index = 0;
            }

            m_SpriteRenderer.sprite = m_Sprites[m_Index];
        }
    }
}
