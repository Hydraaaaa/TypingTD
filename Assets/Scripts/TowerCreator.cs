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

    [SerializeField] GameObject towerDart;
    [SerializeField] GameObject towerSniper;
    [SerializeField] Camera mainCamera;
    [SerializeField] WaveManager waveManager;
    [SerializeField] Text moneyText;

    [Space]

    [SerializeField] int startingMoney = 10;
    [SerializeField] int towerCostDart = 10;
    [SerializeField] int towerCostSniper = 15;

    [HideInInspector]
    [SerializeField] BoolArray[] buildableArea;

    GameObject clone;

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
        if (beenClicked == true)
        {
            Vector3 mousePositionInPixels = Input.mousePosition;
            Vector3 mousePositionInWorld = mainCamera.ScreenToWorldPoint(mousePositionInPixels);

            mousePositionInWorld = new Vector3
            (
                Mathf.Round(mousePositionInWorld.x),
                Mathf.Round(mousePositionInWorld.y),
                mousePositionInWorld.z
            );

            Vector3 clonePosition = new Vector3(mousePositionInWorld.x, mousePositionInWorld.y);
            clone.transform.position = clonePosition;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                beenClicked = false;
            }
        }
    }

    public void MakeTowerDart()
    {
        if (CurrentMoney >= towerCostDart)
        {
            beenClicked = true;
            clone = Instantiate(towerDart, transform.position, Quaternion.identity);

            CurrentMoney -= towerCostDart;
        }
    }
    public void MakeTowerSniper()
    {
        if (CurrentMoney >= towerCostSniper)
        {
            beenClicked = true;
            clone = Instantiate(towerSniper, transform.position, Quaternion.identity);

            CurrentMoney -= towerCostSniper;
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
