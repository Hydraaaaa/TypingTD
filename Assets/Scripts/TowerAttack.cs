using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] int range;
    [SerializeField] float fireRate;
    private float time = 0f;
    public bool towerPlaced = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            towerPlaced = true;
        }
        else if (towerPlaced == true)
        {
            time += Time.deltaTime;
            if (time > fireRate)
            {
                if (Enemy.Enemies != null)
                {
                    for (int i = 0; i < Enemy.Enemies.Count; i++)
                    {
                        if (Vector3.Distance(transform.position, Enemy.Enemies[i].Position) < range)
                        {
                            Enemy.Enemies[i].Health -= 1;
                        }
                    }
                }
                time -= fireRate;
            }
        }
    }
}
