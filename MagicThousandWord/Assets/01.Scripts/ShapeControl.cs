using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeControl : MonoBehaviour
{
    private List<GameObject> shapes = new List<GameObject>();
    private Skybox skybox;
    private int shapeIndex = -1;

    private GameObject brushPoint;

    

    //낮 or 밤 스카이박스 다르게 하기위해
    public enum SkyType
    {
        moon,sun,fire,tree,soil
    };
    public SkyType skyType;

    private void Awake()
    {
        brushPoint = GameObject.Find("brushPoint") as GameObject;
        brushPoint.transform.position = new Vector3(0, -3, 0);
    }

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
        // 해당하는 한자 다 쓰면 각 한자에 맞는 메소드 실행
        if(shapeIndex+1 >= shapes.Count){
            brushPoint.transform.position = new Vector3(0, -3, 0);
            switch (skyType)
            {
                case SkyType.moon:

                    StartCoroutine(skybox.Moon());
                    break;
                case SkyType.sun:
                    StartCoroutine(skybox.Sun());
                    break;
                case SkyType.fire:
                    StartCoroutine(skybox.Fire());
                    break;
                case SkyType.tree:
                    StartCoroutine(skybox.Tree());
                    break;
                case SkyType.soil:
                    StartCoroutine(skybox.Soil());
                    break;
            }
            
            return;
        }
        BoxCollider[] nowCols = shapes[++shapeIndex].GetComponentsInChildren<BoxCollider>();
        brushPoint.transform.position = shapes[shapeIndex].transform.Find("start").transform.position;

        foreach (BoxCollider col in nowCols)
        {
            col.enabled = true;
        }
    }
}
