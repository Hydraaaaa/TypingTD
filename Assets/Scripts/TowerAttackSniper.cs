using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackSniper : Tower
{
    [SerializeField] int range;
    [SerializeField] float fireRate;
    [SerializeField] float fasterFireRate;
    [SerializeField] GameObject shotEffect;
    [SerializeField] int ammoCapacity;
    [SerializeField] int damage;

    float time = 0f;
    bool isFiring = false;
    int shotsFired;

    void Awake()
    {
        TowerPlaced = false;
    }

    void Update()
    {
        if (TowerPlaced == true)
        {
            time += Time.deltaTime;
            if (isFiring == false)
            {
                if (time > fireRate)
                {
                    FireShot();
                    time -= fireRate;
                    isFiring = true;
                    shotsFired += 1;
                }
            }
            else if (isFiring == true)
            {
                if (time > fasterFireRate)
                {
                    FireShot();
                    time -= fasterFireRate;
                    shotsFired += 1;
                    if (shotsFired >= ammoCapacity)
                    {
                       isFiring = false;
                       shotsFired = 0;
                    }
                }
                
            }
        }

        void FireShot()
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
                        Destroy(Instantiate(shotEffect, transform.position, Quaternion.Euler(new Vector3(90, 0, 0))), 3f);
                        _ClosestEnemy.Health -= 1;
                    }
                }
            }
        }
    }
}
