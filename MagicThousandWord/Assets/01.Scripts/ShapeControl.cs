using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeControl : MonoBehaviour
{
    private List<GameObject> shapes = new List<GameObject>();
    private Skybox skybox;
    private int shapeIndex = -1;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            BoxCollider[] colliders = child.gameObject.GetComponentsInChildren<BoxCollider>();
            foreach (BoxCollider col in colliders)
            {
                col.enabled = false;
            }
            shapes.Add(child.gameObject);
        }
        NextShape();

        skybox = GameObject.Find("CenterEyeAnchor").GetComponent<Skybox>();
    }

    public void NextShape()
    {
        if(shapeIndex >= 0){
            BoxCollider[] prevCols = shapes[shapeIndex].GetComponentsInChildren<BoxCollider>();
            foreach (BoxCollider col in prevCols)
            {
                col.enabled = false;
            }
        }

        if(shapeIndex+1 >= shapes.Count){
            StartCoroutine(skybox.Moon());
            //StartCoroutine(skybox.Sun());
            return;
        }

        BoxCollider[] nowCols = shapes[++shapeIndex].GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider col in nowCols)
        {
            col.enabled = true;
        }
    }
}
