using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerCreator : MonoBehaviour
{
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


    [SerializeField] GameObject tower;
    [SerializeField] Camera mainCamera;
    [SerializeField] WaveManager waveManager;
    [SerializeField] Text moneyText;

    [Space]

    [SerializeField] int startingMoney = 10;
    [SerializeField] int towerCost = 10;

    GameObject clone;

    int currentMoney;

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

    public void MakeTower()
    {
        if (CurrentMoney >= towerCost)
        {
            beenClicked = true;
            clone = Instantiate(tower, transform.position, Quaternion.identity);

            CurrentMoney -= towerCost;
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
