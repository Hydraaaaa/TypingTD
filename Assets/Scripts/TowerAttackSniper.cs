using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackSniper : Tower
{
    private IEnumerator Coroutine;
    [SerializeField] int range;
    [SerializeField] float fireRate;
    [SerializeField] GameObject shotEffect;
    [SerializeField] int damage;

    private float time = 0f;

    void Awake()
    {
        TowerPlaced = false;
    }

    void Update()
    {
        if (TowerPlaced == true)
        {
            time += Time.deltaTime;
            if (time > fireRate)
            {
                if (Enemy.Enemies != null &&
                    Enemy.Enemies.Count > 0)
                {
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
                    if (_ClosestDistance < range)
                    {
                        for (int j = 0; j < damage; j++)
                        {
                            //Coroutine = WaitForSeconds(0.1f);
                            Destroy(Instantiate(shotEffect, transform.position, Quaternion.Euler(new Vector3(90, 0, 0))), 3f);
                            _ClosestEnemy.Health -= 1;
                        }
                    }
                }
                time -= fireRate;
            }
        }
    }
}
