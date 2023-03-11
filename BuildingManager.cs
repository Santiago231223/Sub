
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject pendingObject;
    [SerializeField] public Material[] materials;
    

    public Vector3 pos;
     [HideInInspector]public Collider LastHit;
     Collider LastHit2;

    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;
    [SerializeField]private LayerMask anchorLayerMask;
    [SerializeField] private  LayerMask anchorsPillarsLayer;
    public bool canPlace=true;

    public float gridSize;
    public CutHole cutHole;
    public bool cuttingTool;
    bool creatingWall=false;
    bool floorBuild;
    Vector3 aux;
    private bool floorBuilding;
    
    public float YRot;


    public GameObject wall;
    bool creating = false; 

    GameObject startPole;
    GameObject endPole;




    void Update()
    {
        if(pendingObject != null && !cuttingTool)
        {
            buildMode();
        }

        
       
        if (Input.GetMouseButtonDown(0) && cuttingTool)
        {
            cutHole = LastHit.gameObject.GetComponent<CutHole>();
            if(cutHole != null) cutHole.action();

        }

        ChangingModes();

    }

    public void ChangingModes()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DesObject();
            SelectObject(0);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DesObject();
            SelectObject(1);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DesObject();
            floorBuilding = true;
            SelectObject(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            DesObject();
            creatingWall=true;
            SelectObject(3);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DesObject();
            pendingObject = null;
            if (cuttingTool) { cuttingTool = false; }
            if (!cuttingTool)
            {
                cuttingTool = true;
            }

        }
    }

    int i = 1;
    
    public virtual void buildMode()
    {
        if (floorBuilding)
        {
            CheckAnchors();
            
        }
        // floor build para cuando encuentra un anchor y floorbuilding para el estado
        if (!floorBuilding && !creatingWall)
        {
            if (LastHit.tag == "Wall")
            {
                pendingObject.transform.position = new Vector3(pos.x, pos.y, RoundToNearestGrindObj(pos.z));
                pendingObject.transform.rotation = Quaternion.Euler(0, YRot, 0);

            }
            if (LastHit.tag == "Floor")
            {
                
                pendingObject.transform.position = new Vector3(RoundToNearestGrindObj(pos.x), pos.y, RoundToNearestGrindObj(pos.z));
                if (Input.GetKeyDown(KeyCode.R)) { 
                    
                pendingObject.transform.rotation = Quaternion.Euler(0,i*45f, 0);
                    i++;
                    if (i == 8) i = 1;
            }
            }
        } 
            if (floorBuild)
            {
            
            pendingObject.transform.position = LastHit2.gameObject.transform.position + aux; aux = new Vector3(0, 0, 0);
                pendingObject.transform.rotation = Quaternion.identity;
            }
            if(creatingWall) {
            CheckPillarAnchors();
            if (anchorHit != null) { 
                
                pendingObject.transform.position = anchorHit.gameObject.transform.position;
                pendingObject.transform.position = new Vector3(pendingObject.transform.position.x,pendingObject.transform.position.y+2,pendingObject.transform.position.z);
               
            }

            else { pendingObject.transform.position = new Vector3(pos.x, pos.y + 2, pos.z); }


        }

        UpdateMaterials();

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
               if(!creatingWall) placeObject();
            
            }
        if (creatingWall)
        {
            
            if (!creating && (Input.GetMouseButtonDown(0)))
            {
                startFence();
                creating = true;

            }
            else if (Input.GetMouseButtonDown(0))
            {
                endFence();

                adjustWall();
                creating = false;
            }



        }
    }

    Collider anchorHit;
    public void CheckPillarAnchors()
    {
        Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray3, out hit, 5, anchorsPillarsLayer))
        {
        
            anchorHit = hit.collider;
        }

    }
    private void FixedUpdate()
    {
        GetPos();
       
    }
    float RoundToNearestGrindObj(float pos)
    {
        float xDiff = pos % gridSize;
        pos -= xDiff;
        if(xDiff > (gridSize / 2))
        {
            pos += gridSize;
        }
        return pos;
    }
    public void CheckAnchors()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       
        if (Physics.Raycast(ray, out hit, 5, anchorLayerMask))
        {
            LastHit2 = hit.collider;

            switch (LastHit2.gameObject.name)
            {
                case "WestCollider":
                    floorBuild = true;
                    aux = new Vector3(0, 0, -2.5f);
                    break;
                case "EastCollider":
                    aux = new Vector3(0, 0, +2.5f); 
                    floorBuild = true;
                    break;
                case "NorthCollider":
                    aux = new Vector3(-2.5f, 0, 0);
                    floorBuild = true;
                    break;
                case "SouthCollider":
                    aux = new Vector3(2.5f, 0, 0);
                    floorBuild = true;
                    break;
            }
        }
    }

    private void startFence()
    {
        Vector3 FenceStartPos = pos;

        startPole = pendingObject;
        //placeobject pero aqui
        startPole.GetComponentInChildren<Collider>().gameObject.layer = 0;
        startPole.GetComponentInChildren<Renderer>().material = materials[2];


        pendingObject = null;
    }
    public void endFence()
    {
        Vector3 FenceEndPos = pos;
        endPole = pendingObject;
        //place object pero aqui
        endPole.GetComponentInChildren<Collider>().gameObject.layer = 0;//temporal
        endPole.GetComponentInChildren<Renderer>().material = materials[2];
       
        pendingObject = null;

    }
    private void adjustWall()
    {
        startPole.transform.LookAt(endPole.transform.position);
        endPole.transform.LookAt(startPole.transform.position);
        wall = Instantiate(wall, startPole.transform.position, Quaternion.identity);
        GameObject wall2 = wall;
        wall2.transform.position = startPole.transform.position + (Vector3.Distance(startPole.transform.position, endPole.transform.position) / 2) * startPole.transform.forward;
        wall2.transform.rotation = startPole.transform.rotation;
        wall2.transform.localScale = new Vector3(wall2.transform.localScale.x, wall2.transform.localScale.y, Vector3.Distance(startPole.transform.position, endPole.transform.position));

    }

    public void UpdateMaterials()
    {
        if (canPlace) { pendingObject.GetComponentInChildren<Renderer>().material = materials[0]; }
        if (!canPlace) { pendingObject.GetComponentInChildren<Renderer>().material = materials[1]; }
    }

    public void DesObject()
    {
        cuttingTool = false;
        floorBuilding = false;
        floorBuild = false;
        creatingWall = false;

        if (pendingObject != null)
        {
            Destroy(pendingObject);
        }
    }

    public void SelectObject(int index)
    {
        pendingObject = Instantiate(objects[index], pos, transform.rotation);
        pendingObject.GetComponentInChildren<Collider>().gameObject.layer = 11;

    }

    public void GetPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 5, layerMask))
        {
            pos = hit.point;
            YRot = hit.transform.rotation.eulerAngles.y;
            LastHit = hit.collider;
            pos += new Vector3(0, 0, 0);

        }
    }

       public virtual void placeObject()
    {
        pendingObject.GetComponentInChildren<Collider>().gameObject.layer = 9;
        pendingObject.GetComponentInChildren<Renderer>().material = materials[2];
        pendingObject = null;
    }

}
