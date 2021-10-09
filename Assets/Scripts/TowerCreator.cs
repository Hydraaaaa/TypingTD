using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCreator : MonoBehaviour
{
    GameObject clone;
    public bool beenClicked = false;
    [SerializeField] GameObject tower;
    [SerializeField] Camera mainCamera;
    void Start()
    {
        
    }

    public void MakeCube()
    {
        beenClicked = true;
        clone = Instantiate(tower, transform.position, Quaternion.identity);
    }

    void Update()
    {
        if (beenClicked == true)
        {
            Vector3 mousePositionInPixels = Input.mousePosition;
            Vector3 mousePositionInWorld = mainCamera.ScreenToWorldPoint(mousePositionInPixels);
            Vector3 clonePosition = new Vector3(mousePositionInWorld.x, mousePositionInWorld.y);
            clone.transform.position = clonePosition;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                beenClicked = false;
            }
        }
    }
}
