using UnityEngine;
using UnityEngine.Rendering.Universal;


public class LightManager : MonoBehaviour
{
    Light2D myLight;
    float originalIntensity;

    public float radiusA;
    public float radiusB;
    public bool canFlicker;



    void Start()
    {
        myLight = GetComponent<Light2D>();
        originalIntensity = myLight.intensity;
    }

    void Update()
    {
        if (canFlicker)
        {
            myLight.pointLightOuterRadius = Random.Range(radiusA, radiusB);
        }

    }


}