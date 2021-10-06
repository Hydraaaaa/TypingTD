using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public event Action<int> OnWaveStarted;
    public event Action OnWaveEnded;

    public bool IsWaveActive { get; private set; }

    int m_CurrentWave = -1;

    List<EnemySpawner> m_EnemySpawners = new List<EnemySpawner>();

    void Awake()
    {
        IsWaveActive = false;
    }

    void Update()
    {
        if (IsWaveActive)
        {
            IsWaveActive = IsWaveStillActive();

            if (!IsWaveActive)
            {
                OnWaveEnded?.Invoke();
            }
        }
    }

    bool IsWaveStillActive()
    {
        if (Enemy.Enemies != null &&
            Enemy.Enemies.Count > 0)
        {
            return true;
        }

        for (int i = 0; i < m_EnemySpawners.Count; i++)
        {
            if (m_EnemySpawners[i].IsSpawning)
            {
                return true;
            }
        }

        return false;
    }

    public void RegisterEnemySpawner(EnemySpawner a_EnemySpawner)
    {
        m_EnemySpawners.Add(a_EnemySpawner);
    }

    public void UnregisterEnemySpawner(EnemySpawner a_EnemySpawner)
    {
        m_EnemySpawners.Remove(a_EnemySpawner);
    }

    public void StartNextWave()
    {
        if (!IsWaveActive)
        {
            m_CurrentWave++;
            IsWaveActive = true;

            for (int i = 0; i < m_EnemySpawners.Count; i++)
            {
                m_EnemySpawners[i].SpawnWave(m_CurrentWave);
            }

            OnWaveStarted?.Invoke(m_CurrentWave);
        }
    }
}
