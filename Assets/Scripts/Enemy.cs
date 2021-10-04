using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float m_MovementSpeed = 1.0f;

    public event Action OnDestinationReached;

    Vector2[] m_Waypoints;

    Vector3 m_MovementDirection;

    int m_CurrentWaypoint;

    bool m_IsMoving;

    public void Initialize(Vector2[] a_Waypoints)
    {
        m_Waypoints = a_Waypoints;

        m_CurrentWaypoint = 0;

        m_MovementDirection = (new Vector3(a_Waypoints[0].x, a_Waypoints[0].y, transform.position.z) - transform.position).normalized;

        m_IsMoving = true;
    }

    void Update()
    {
        if (m_IsMoving)
        {
            Move();
        }
    }

    void Move()
    {
        float _FrameMovement = m_MovementSpeed * Time.deltaTime;
        float _DistanceToWaypoint = Vector2.Distance(transform.position, m_Waypoints[m_CurrentWaypoint]);

        // If the travel distance would allow the enemy to reach the waypoint
        while (_FrameMovement >= _DistanceToWaypoint)
        {
            _FrameMovement -= _DistanceToWaypoint;

            transform.position = m_Waypoints[m_CurrentWaypoint];

            m_CurrentWaypoint++;

            if (m_CurrentWaypoint == m_Waypoints.Length)
            {
                m_IsMoving = false;

                DestinationReached();

                return;
            }

            _DistanceToWaypoint = Vector2.Distance(transform.position, m_Waypoints[m_CurrentWaypoint]);

            m_MovementDirection = (new Vector3(m_Waypoints[m_CurrentWaypoint].x, m_Waypoints[m_CurrentWaypoint].y, transform.position.z) - transform.position).normalized;
        }

        transform.position += m_MovementDirection * _FrameMovement;
    }

    void DestinationReached()
    {
        OnDestinationReached?.Invoke();
        Destroy(gameObject);
    }
}
