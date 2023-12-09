using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEnter : MonoBehaviour
{
    public Material skyboxMaterial; 
    public Color caveTint = Color.black; 
    public Color caveFogColor = Color.black;
    public float caveFogDensity;

    private Color originalFogColor;
    private float originalFogDensity;

    float timeLeft;

    private bool darkening, brightening;

    private void Start()
    {
        originalFogColor = RenderSettings.fogColor;
        originalFogDensity = 200;
        timeLeft = 2.0f;
    }

   

    void Update()
    {
        if (darkening)
        {
            if (timeLeft <= Time.deltaTime)
            {

                RenderSettings.fogColor = caveFogColor;
                RenderSettings.fogEndDistance = caveFogDensity;
                darkening = false;
                timeLeft = 2.0f;
            }
            else
            {
                RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, caveFogColor, Time.deltaTime / timeLeft);
                RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, caveFogDensity, Time.deltaTime / timeLeft);

                // update the timer
                timeLeft -= Time.deltaTime;
            }
        }

        if(brightening)
        {
            if (timeLeft <= Time.deltaTime)
            {
                RenderSettings.fogColor = originalFogColor;
                RenderSettings.fogEndDistance = originalFogDensity;
                brightening = false;
                timeLeft = 2.0f;
            }
            else
            {
                RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, originalFogColor, Time.deltaTime / timeLeft);
                RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogDensity, originalFogDensity, Time.deltaTime / timeLeft);

                // update the timer
                timeLeft -= Time.deltaTime;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Darken();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Brighten();
        }
    }

    private void Darken()
    {
        //brightening = false;
        //darkening = true;
        //timeLeft = 2.0f;
        RenderSettings.fogColor = caveFogColor;
        RenderSettings.fogEndDistance = caveFogDensity;
    }

    private void Brighten()
    {
        //darkening = false;
        //brightening = true;
        //timeLeft = 2.0f;
        RenderSettings.fogColor = originalFogColor;
        RenderSettings.fogEndDistance = originalFogDensity;
    }
}
