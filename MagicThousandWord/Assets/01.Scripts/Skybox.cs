using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skybox : MonoBehaviour
{

    public Material skybox1;
    public Material skybox2;
    public Light light1;
    float intensity;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = skybox1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator Moon()
    {
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
    }

    public IEnumerator Sun()
    {
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

    }
}
