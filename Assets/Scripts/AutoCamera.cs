using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCamera : MonoBehaviour
{
    [SerializeField] Camera m_Camera;
    [SerializeField] WaveManager m_WaveManager;
    [SerializeField] float m_MinZoom = 5.0f;
    [SerializeField] float m_ViewPadding = 1.0f;
    [SerializeField] float m_MapWidth;
    [SerializeField] float m_MapHeight;
    [SerializeField] float m_TrackSpeed = 4.0f;

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
                        _MinX = _Enemies[i].transform.position.x - _Enemies[i].WordWidth / 2;
                        _MaxX = _Enemies[i].transform.position.x + _Enemies[i].WordWidth / 2;
                        _MinY = _Enemies[i].transform.position.y - _Enemies[i].WordHeight / 2;
                        _MaxY = _Enemies[i].transform.position.y + _Enemies[i].WordHeight / 2;

                        _InitialValueSet = true;
                    }
                    else
                    {
                        if (_Enemies[i].transform.position.x - _Enemies[i].WordWidth / 2 < _MinX)
                        {
                            _MinX = _Enemies[i].transform.position.x - _Enemies[i].WordWidth / 2;
                        }

                        if (_Enemies[i].transform.position.x + _Enemies[i].WordWidth / 2 > _MaxX)
                        {
                            _MaxX = _Enemies[i].transform.position.x + _Enemies[i].WordWidth / 2;
                        }

                        if (_Enemies[i].transform.position.y - _Enemies[i].WordHeight / 2 < _MinY)
                        {
                            _MinY = _Enemies[i].transform.position.y - _Enemies[i].WordHeight / 2;
                        }

                        if (_Enemies[i].transform.position.y + _Enemies[i].WordHeight / 2 > _MaxY)
                        {
                            _MaxY = _Enemies[i].transform.position.y + _Enemies[i].WordHeight / 2;
                        }
                    }
                }

                // Even though an enemy won't be counted if it's not inside the map bounds
                // The offset of text can push the min/max values outside the map bounds
                _MinX = Mathf.Clamp(_MinX, -m_MapWidth, m_MapWidth);
                _MaxX = Mathf.Clamp(_MaxX, -m_MapWidth, m_MapWidth);
                _MinY = Mathf.Clamp(_MinY, -m_MapHeight, m_MapHeight);
                _MaxY = Mathf.Clamp(_MaxY, -m_MapHeight, m_MapHeight);

                // If there are any enemies to follow
                // Can conveniently reuse this bool which happens to align with our criteria
                if (_InitialValueSet)
                {
                    Vector2 _CenterPos = new Vector2((_MinX + _MaxX) / 2, (_MinY + _MaxY) / 2);

                    float _FurthestXDistance = Mathf.Abs(_MaxX - _CenterPos.x) + m_ViewPadding;
                    float _FurthestYDistance = Mathf.Abs(_MaxY - _CenterPos.y) + m_ViewPadding;

                    // Compensate for aspect ratio
                    // If two enemies are split on the X axis, we have a lot more screen real estate to work with, so we need to zoom out later than on the Y axis
                    _FurthestXDistance *= (float)Screen.height / Screen.width;

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

                    float _XDifference = m_Position.x - transform.position.x;
                    float _YDifference = m_Position.y - transform.position.y;

                    m_Camera.orthographicSize += _ZoomDifference * Time.deltaTime * m_TrackSpeed;

                    transform.position += new Vector3
                    (
                        _XDifference * Time.deltaTime * m_TrackSpeed,
                        _YDifference * Time.deltaTime * m_TrackSpeed,
                        0
                    );

                    //m_Camera.orthographicSize = m_Zoom;

                    //transform.position += new Vector3
                    //(
                        //_XDifference,
                        //_YDifference,
                        //0
                    //);
                }
            }
        }
    }
}
