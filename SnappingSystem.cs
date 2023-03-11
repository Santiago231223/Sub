using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SnappingSystem : MonoBehaviour
{
    BuildingManager buildingManager;
    bool floorBuild;
    public LayerMask layerMask;
    Collider LastHit;
    void Start()
    {
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if(floorBuild) floorBuild = false;
            if (!floorBuild) floorBuild = true;
        }

        if (Input.GetMouseButtonDown(0) && floorBuild)
        {

            Snap();
        }
        
    }

    public void Snap()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5, layerMask))
        {
            LastHit = hit.collider;

            Debug.Log("Aqui si");
            switch (LastHit.gameObject.name)
            {
                case "WestCollider":
                    Instantiate(buildingManager.objects[2], LastHit.gameObject.transform.position, Quaternion.identity);
                    break;
                case "EastCollider":
                    Instantiate(buildingManager.objects[2], LastHit.gameObject.transform.position, Quaternion.identity);
                    break;
                case "NorthCollider":
                    Instantiate(buildingManager.objects[2], LastHit.gameObject.transform.position, Quaternion.identity);
                    break;
                case "SouthCollider":
                    Instantiate(buildingManager.objects[2], LastHit.gameObject.transform.position, Quaternion.identity);
                    break;

            }
        }
    }


}
