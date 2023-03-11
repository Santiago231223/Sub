using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutHole : MonoBehaviour
{
   
        BuildingManager buildingManager;
        void Start()
        {
            buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        }
    

    // Update is called once per frame
   
    

    public void deleteTri(int index)
    {

        Destroy(this.gameObject.GetComponent<MeshCollider>());
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[mesh.triangles.Length-3];

        int i = 0;
        int j = 0;

        while (j < mesh.triangles.Length)
        {
            if(j != index * 3)
            {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            }
            else
            {
                j += 3;
            }

        }
        transform.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
        this.gameObject.AddComponent<MeshCollider>();
    }
    public void action()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            deleteTri(hit.triangleIndex);

        }
    }



    }
