using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCamera : MonoBehaviour
{
    [SerializeField] Camera m_Camera;
    [SerializeField] WaveManager m_WaveManager;
    [SerializeField] float m_MinZoom = 5.0f;
    [SerializeField] float m_MapWidth;
    [SerializeField] float m_MapHeight;

    Vector3 m_Position;
    float m_Zoom = 5.0f;

    void Update()
    {
        if (m_WaveManager.IsWaveActive)
        {
            List<Enemy> _Enemies = Enemy.Enemies;

            if (_Enemies != null)
            {
                bool _InitialValueSet = false;

                float _MinX = 0;
                float _MinY = 0;
                float _MaxX = 0;
                float _MaxY = 0;

                for (int i = 0; i < _Enemies.Count; i++)
                {
                    if (Mathf.Abs(_Enemies[i].transform.position.x) > m_MapWidth ||
                        Mathf.Abs(_Enemies[i].transform.position.y) > m_MapHeight)
                    {
                        continue;
                    }

                    // Need to get base values for the extents of enemies, use the first enemy in the loop and trigger this bool
                    if (!_InitialValueSet)
                    {
                        _MinX = _Enemies[i].transform.position.x;
                        _MaxX = _Enemies[i].transform.position.x;
                        _MinY = _Enemies[i].transform.position.y;
                        _MaxY = _Enemies[i].transform.position.y;

                        _InitialValueSet = true;
                    }
                    else
                    {
                        if (_Enemies[i].transform.position.x < _MinX)
                        {
                            _MinX = _Enemies[i].transform.position.x;
                        }

                        if (_Enemies[i].transform.position.x > _MaxX)
                        {
                            _MaxX = _Enemies[i].transform.position.x;
                        }

                        if (_Enemies[i].transform.position.y < _MinY)
                        {
                            _MinY = _Enemies[i].transform.position.y;
                        }

                        if (_Enemies[i].transform.position.y > _MaxY)
                        {
                            _MaxY = _Enemies[i].transform.position.y;
                        }
                    }
                }

                // If there are any enemies to follow
                // Can conveniently reuse this bool which happens to align with our criteria
                if (_InitialValueSet)
                {
                    Vector2 _CenterPos = new Vector2((_MinX + _MaxX) / 2, (_MinY + _MaxY) / 2);

                    float _FurthestXDistance = Mathf.Abs(_MaxX - _CenterPos.x);
                    float _FurthestYDistance = Mathf.Abs(_MaxY - _CenterPos.y);

                    // Compensate for aspect ratio
                    // If two enemies are split on the Y axis, we have a lot less screen real estate to work with, so we need to zoom out sooner than on the X axis
                    _FurthestYDistance *= (float)Screen.width / Screen.height;

                    float _FurthestDistance;

                    if (_FurthestXDistance > _FurthestYDistance)
                    {
                        _FurthestDistance = _FurthestXDistance;
                    }
                    else
                    {
                        _FurthestDistance = _FurthestYDistance;
                    }

                    m_Position = new Vector3((_MinX + _MaxX) / 2, (_MinY + _MaxY) / 2, transform.position.z);

                    m_Zoom = Mathf.Max(m_MinZoom, _FurthestDistance);

                    float _ZoomDifference = m_Zoom - m_Camera.orthographicSize;

                    m_Camera.orthographicSize += _ZoomDifference * Time.deltaTime / 2;

                    float _XDifference = m_Position.x - transform.position.x;
                    float _YDifference = m_Position.y - transform.position.y;

                    m_Camera.orthographicSize += _ZoomDifference * Time.deltaTime / 2;

                    transform.position += new Vector3
                    (
                        _XDifference * Time.deltaTime,
                        _YDifference * Time.deltaTime,
                        0
                    );
                }
            }
        }
    }
}
