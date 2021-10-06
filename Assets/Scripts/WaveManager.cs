using System;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WaveManager : MonoBehaviour
{
    [SerializeField] Waypoint[] m_Waypoints;
    [SerializeField] Wave[] m_Waves;

    int m_CurrentWave = -1;

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

    public static void StartNextWave()
    {
        OnWaveStarted?.Invoke();
    }

    public static event Action OnWaveStarted;

    void Awake()
    {
        OnWaveStarted += SpawnNextWave;
    }

    void OnDestroy()
    {
        OnWaveStarted -= SpawnNextWave;
    }

    void SpawnNextWave()
    {
        m_CurrentWave++;

        StartCoroutine(SpawnWaveCoroutine(m_Waves[m_CurrentWave]));
    }

    IEnumerator SpawnWaveCoroutine(Wave a_Wave)
    {
        for (int i = 0; i < a_Wave.EnemyGroups.Length; i++)
        {
            yield return new WaitForSeconds(a_Wave.EnemyGroups[i].SpawnDelay);

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
    }

    [Serializable]
    public class Wave
    {
        public EnemyGroup[] EnemyGroups;
    }

    [Serializable]
    public class EnemyGroup
    {
        public Enemy Enemy;
        public int Amount;
        public float SpawnInterval; // Time between spawns within this group
        public float SpawnDelay; // Time before this group starts spawning
    }
}