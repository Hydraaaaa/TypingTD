using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerCreator : MonoBehaviour
{
    [Serializable]
    public class BoolArray
    {
        public bool[] Array;
    }

    public bool beenClicked = false;

    public int CurrentMoney
    {
        get { return currentMoney; }
        set
        {
            currentMoney = value;
            moneyText.text = value.ToString();
        }
    }

    [SerializeField] Tower towerDart;
    [SerializeField] Tower towerSniper;
    [SerializeField] Camera mainCamera;
    [SerializeField] WaveManager waveManager;
    [SerializeField] Text moneyText;

    [Space]

    [SerializeField] int startingMoney = 10;

    [HideInInspector]
    [SerializeField] BoolArray[] buildableArea;

    Tower clone;

    int currentMoney;

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        int width = buildableArea.Length;
        int height = buildableArea[0].Array.Length;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xPos = (x - width / 2.0f) + 0.5f;
                float yPos = (y - height / 2.0f) + 0.5f;

                Color _Color = Color.red;

                if (buildableArea[x].Array[y])
                {
                    _Color = Color.blue;
                }

                _Color.a = 0.4f;

                Gizmos.color = _Color;

                Gizmos.DrawCube(new Vector3(xPos, yPos, 0), new Vector3(0.5f, 0.5f, 0.01f));
            }
        }
    }
#endif

    void Start()
    {
        CurrentMoney = startingMoney;

        moneyText.text = CurrentMoney.ToString();

        for (int i = 0; i < waveManager.EnemySpawners.Count; i++)
        {
            waveManager.EnemySpawners[i].OnEnemySpawned += OnEnemySpawned;
        }
    }

    void Update()
    {
        Vector3 mousePositionInPixels = Input.mousePosition;
        Vector3 mousePositionInWorld = mainCamera.ScreenToWorldPoint(mousePositionInPixels);

        int xIndex = Mathf.RoundToInt(mousePositionInWorld.x + buildableArea.Length / 2.0f);
        int yIndex = Mathf.RoundToInt(mousePositionInWorld.y + buildableArea[0].Array.Length / 2.0f);

        mousePositionInWorld = new Vector3
        (
            Mathf.Round(mousePositionInWorld.x),
            Mathf.Round(mousePositionInWorld.y),
            mousePositionInWorld.z
        );

        if (beenClicked == true)
        {
            if (xIndex >= 1 &&
                yIndex >= 1 &&
                xIndex < buildableArea.Length &&
                yIndex < buildableArea[0].Array.Length &&
                buildableArea[xIndex].Array[yIndex] &&
                buildableArea[xIndex - 1].Array[yIndex] &&
                buildableArea[xIndex - 1].Array[yIndex - 1] &&
                buildableArea[xIndex].Array[yIndex - 1])
            {
                Vector3 clonePosition = new Vector3(mousePositionInWorld.x, mousePositionInWorld.y);
                clone.transform.position = clonePosition;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                beenClicked = false;
            }
        }
    }

    public void MakeTower(Tower a_Tower)
    {
        if (CurrentMoney >= a_Tower.Cost)
        {
            beenClicked = true;
            clone = Instantiate(a_Tower, transform.position, Quaternion.identity);

            CurrentMoney -= a_Tower.Cost;

            moneyText.text = CurrentMoney.ToString();
        }
    }

    void OnEnemySpawned(Enemy a_Enemy)
    {
        a_Enemy.OnDeath += OnEnemyDeath;
    }

    void OnEnemyDeath(Enemy a_Enemy)
    {
        CurrentMoney += a_Enemy.KillReward;
    }
}
