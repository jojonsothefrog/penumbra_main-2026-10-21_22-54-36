using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;

public class LightInteractions : MonoBehaviour
{
    //  Fazer sistema que coloque uma propriedade pros objetos de luz da unity
    //  Passar por todos e dar esse attribui
    [SerializeField]
    private List<Light2D> lights = new List<Light2D>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(Transform child in transform)
        {
            Light2D light = child.GetComponent<Light2D>();

            if (child != null)
            {
                lights.Add(light);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void turnOnLights()
    {
        foreach(Light2D light in lights)
        {
            if (!light.enabled)
            {
                light.enabled = true;
            }
        }
    }

    void turnOfflights(int Lights)
    {
        int lightAmount = Lights; // Número de luzes pra apagar

        foreach(Light2D light in lights)
        {
            if (lightAmount <= 0)
                break;

            if (light.enabled)
            {
                light.enabled = false;
                lightAmount--;
            }

        }
    }
}
