using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageAnimation : MonoBehaviour
{
    [SerializeField] Image m_Image;
    [SerializeField] Sprite[] m_Sprites;

    [Space]

    public float Interval = 0.2f;
    [SerializeField] bool m_PlayOnAwake;
    public bool Loop;

    float m_CurrentInterval = 0.0f;
    int m_Index = 0;
    bool m_IsPlaying = false;

    void Reset()
    {
        m_Image = GetComponent<Image>();
    }

    void Awake()
    {
        m_IsPlaying = m_PlayOnAwake;

        if (m_Sprites.Length > 0)
        {
            m_Image.sprite = m_Sprites[0];
        }
    }

    void Update()
    {
        if (m_Sprites.Length == 0)
        {
            if (m_IsPlaying)
            {
                Debug.LogError("Attempted to play animation without any sprites");
            }

            m_IsPlaying = false;
            m_Index = 0;
            m_CurrentInterval = 0.0f;
        }

        if (m_IsPlaying)
        {
            m_CurrentInterval += Time.deltaTime;

            if (m_CurrentInterval >= Interval)
            {
                m_Index++;

                m_CurrentInterval -= Interval;

                if (m_Index >= m_Sprites.Length)
                {
                    m_Index = 0;

                    if (!Loop)
                    {
                        m_IsPlaying = false;
                        m_CurrentInterval = 0.0f;
                    }
                    else
                    {
                        m_Image.sprite = m_Sprites[m_Index];
                    }
                }
                else
                {
                    m_Image.sprite = m_Sprites[m_Index];
                }
            }
        }
    }

    public void Play()
    {
        m_IsPlaying = true;
    }

    public void Pause()
    {
        m_IsPlaying = false;
    }

    public void Stop()
    {
        m_IsPlaying = false;

        m_Index = 0;
        m_CurrentInterval = 0.0f;
    }

    public void SetFrame(int a_Frame)
    {
        m_Index = Mathf.Clamp(a_Frame, 0, m_Sprites.Length - 1);
        m_CurrentInterval = 0.0f;
    }
}
