using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            for (int i = 0; i < Enemy.Enemies.Count; i++)
            {
                Debug.Log(Vector3.Distance(transform.position, Enemy.Enemies[i].transform.position));
                Enemy.Enemies[i].Health -= 1;
            }
            //so for every enemy that reaches within the specified distance attack it for 1 damage (hi hydra)
        }
    }
}
