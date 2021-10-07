using System;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] WaveManager m_WaveManager;
    [SerializeField] Waypoint[] m_Waypoints;
    [SerializeField] Wave[] m_Waves;

    public bool IsSpawning { get; private set; }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        GUIStyle _Style = new GUIStyle();

        _Style.alignment = TextAnchor.UpperCenter;
        _Style.fontSize = 20;

        Gizmos.color = Color.yellow;

        Handles.Label(transform.position + new Vector3(0, 0.5f, 0), "Spawn", _Style);

        Gizmos.DrawSphere(transform.position, 0.1f);

        Vector3 _WaypointPos = transform.position;
        Vector3 _PreviousWaypointPos;

        for (int i = 0; i < m_Waypoints.Length; i++)
        {
            if (m_Waypoints[i].AreEnemiesTargetable)
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.black;
            }

            _PreviousWaypointPos = _WaypointPos;
            _WaypointPos = new Vector3(m_Waypoints[i].Position.x, m_Waypoints[i].Position.y, transform.position.z);

            Handles.Label(_WaypointPos + new Vector3(0, 0.5f, 0), $"{i}", _Style);

            Gizmos.DrawSphere(_WaypointPos, 0.1f);
            Gizmos.DrawLine(_PreviousWaypointPos, _WaypointPos);
        }
    }
#endif

    void Awake()
    {
        m_WaveManager.RegisterEnemySpawner(this);
    }

    void OnDestroy()
    {
        m_WaveManager.UnregisterEnemySpawner(this);
    }

    public void SpawnWave(int a_Wave)
    {
        StartCoroutine(SpawnWaveCoroutine(m_Waves[a_Wave]));
    }

    IEnumerator SpawnWaveCoroutine(Wave a_Wave)
    {
        IsSpawning = true;

        for (int i = 0; i < a_Wave.EnemyGroups.Length; i++)
        {
            yield return new WaitForSeconds(a_Wave.EnemyGroups[i].InitialDelay);

            for (int j = 0; j < a_Wave.EnemyGroups[i].Amount; j++)
            {
                Enemy _Enemy = Instantiate(a_Wave.EnemyGroups[i].Enemy, transform.position, Quaternion.identity);

                _Enemy.Initialize(m_Waypoints);

                if (j != a_Wave.EnemyGroups[i].Amount - 1)
                {
                    yield return new WaitForSeconds(a_Wave.EnemyGroups[i].SpawnInterval);
                }
            }
        }

        IsSpawning = false;
    }

    [Serializable]
    public class Wave
    {
        public EnemyGroup[] EnemyGroups;
    }

    [Serializable]
    public class EnemyGroup
    {
        [Tooltip("Time before this group starts spawning")]
        public float InitialDelay;
        public Enemy Enemy;
        public int Amount;
        [Tooltip("Time between spawns within this group")]
        public float SpawnInterval;
    }
}