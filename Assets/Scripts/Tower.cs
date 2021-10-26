using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public int Cost => cost;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] int cost;

    protected bool TowerPlaced = false;

    public void Initialize()
    {
        TowerPlaced = true;
    }
}
