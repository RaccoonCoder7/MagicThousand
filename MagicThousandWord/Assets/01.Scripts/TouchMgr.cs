using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchMgr : MonoBehaviour
{
    private Transform tr;
    private LineRenderer line;

    private Ray ray;
    private RaycastHit hit;
    private Camera cam;
    private int layerBT;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        line = GetComponent<LineRenderer>();
        layerBT = 1 << LayerMask.NameToLayer("DIALOGUE");
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(tr.position, tr.forward);
        if (Physics.Raycast(ray, out hit, 16.0f))
        {
            float dist = hit.distance;
            line.SetPosition(1, new Vector3(0, 0, dist));
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (Physics.Raycast(ray, out hit, 100.0f, layerBT))
            {
                hit.collider.gameObject.SetActive(false);
            }
        }
    }
}
