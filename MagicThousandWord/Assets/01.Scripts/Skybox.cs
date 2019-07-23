using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skybox : MonoBehaviour
{

    public Material skybox1;
    public Material skybox2;
    public Light light1;
    float intensity;
    public GameObject fireEffect;
    public GameObject treeEffect;
    public GameObject soilEffect;
    public MeshRenderer land;
    public Texture tx;
    private TouchMgr touchMgr;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = skybox1;
        touchMgr = GameObject.FindGameObjectWithTag("TouchMgr").GetComponent<TouchMgr>();
        anim = GameObject.Find("NPC").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator Moon()
    {
        touchMgr.skipObj.SetActive(false);
        intensity = light1.intensity;
        while (intensity >=0)
        {
            intensity -= 0.05f;
            light1.intensity = intensity;
            yield return new WaitForSeconds(0.1f);
        }
        RenderSettings.skybox = skybox2;
        yield return new WaitForSeconds(1.0f);
        while (intensity <= 1.0f)
        {
            intensity += 0.05f;
            light1.intensity = intensity;
            yield return new WaitForSeconds(0.1f);
        }
        touchMgr.EndDrawing();
    }

    public IEnumerator Sun()
    {
        touchMgr.skipObj.SetActive(false);
        intensity = light1.intensity;
        while (intensity >= 0)
        {
            intensity -= 0.05f;
            light1.intensity = intensity;
            yield return new WaitForSeconds(0.1f);
        }
        RenderSettings.skybox = skybox1;
        yield return new WaitForSeconds(1.0f);
        while (intensity <= 1.0f)
        {
            intensity += 0.05f;
            light1.intensity = intensity;
            yield return new WaitForSeconds(0.1f);
        }
        touchMgr.EndDrawing();
    }

    public IEnumerator Fire()
    {
        touchMgr.skipObj.SetActive(false);
        fireEffect.SetActive(true);
        anim.SetTrigger("ATTACKED");
        yield return new WaitForSeconds(3.0f);
        Destroy(fireEffect);
        touchMgr.EndDrawing();
    }
    public IEnumerator Tree()
    {
        touchMgr.skipObj.SetActive(false);
        treeEffect.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        touchMgr.EndDrawing();
    }
    public IEnumerator Soil()
    {
        touchMgr.skipObj.SetActive(false);
        soilEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        land.material.SetTexture("_MainTex",tx);
        yield return new WaitForSeconds(1.5f);
        Destroy(soilEffect);
        touchMgr.EndDrawing();
    }
}
