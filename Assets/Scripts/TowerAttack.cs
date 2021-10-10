using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public bool TowerPlaced;

    [SerializeField] int range;
    [SerializeField] float fireRate;

    private float time = 0f;

    void Awake()
    {
        TowerPlaced = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TowerPlaced = true;
        }
        else if (TowerPlaced == true)
        {
            time += Time.deltaTime;
            if (time > fireRate)
            {
                if (Enemy.Enemies != null &&
                    Enemy.Enemies.Count > 0)
                {
                    Debug.Log($"!! TEST");
                    Enemy _ClosestEnemy = Enemy.Enemies[0];
                    float _ClosestDistance = Vector3.Distance(transform.position, Enemy.Enemies[0].Position);

                    for (int i = 1; i < Enemy.Enemies.Count; i++)
                    {
                        float _Distance = Vector3.Distance(transform.position, Enemy.Enemies[i].Position);

                        if (_Distance < _ClosestDistance)
                        {
                            _ClosestEnemy = Enemy.Enemies[i];
                            _ClosestDistance = _Distance;
                        }
                    }

                    _ClosestEnemy.Health -= 1;
                }

                time -= fireRate;
            }
        }
    }
}
