using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int Cost;
    protected bool TowerPlaced;


    protected void CheckIfPlaced()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TowerPlaced = true;
        }
    }
}
