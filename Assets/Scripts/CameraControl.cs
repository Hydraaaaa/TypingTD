using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Camera m_Camera;
    [SerializeField] WaveManager m_WaveManager;
    [SerializeField] float m_ScrollSpeed = 1.0f;

    bool m_Active;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            m_Active = false;
        }

        if (!m_WaveManager.IsWaveActive)
        {
            m_Camera.orthographicSize -= Input.mouseScrollDelta.y;

            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Confined;
                m_Active = true;
            }

            if (m_Active)
            {
                Vector3 _Position = transform.position;

                if (Input.mousePosition.x <= 2)
                {
                    _Position.x -= Time.deltaTime * m_ScrollSpeed;
                }
                else if (Input.mousePosition.x >= Screen.width - 3)
                {
                    _Position.x += Time.deltaTime * m_ScrollSpeed;
                }

                if (Input.mousePosition.y <= 2)
                {
                    _Position.y -= Time.deltaTime * m_ScrollSpeed;
                }
                else if (Input.mousePosition.y >= Screen.height - 3)
                {
                    _Position.y += Time.deltaTime * m_ScrollSpeed;
                }

                _Position.x = Mathf.Clamp(_Position.x, -20, 20);
                _Position.y = Mathf.Clamp(_Position.y, -20, 20);

                transform.position = _Position;
            }
        }
    }
}
