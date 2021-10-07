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
            Debug.Log("y");
            for (int i = 0; i < Enemy.Enemies.Count; i++)
            {
                Enemy.Enemies[i].Health -= 2;
            }
        }
    }
}
