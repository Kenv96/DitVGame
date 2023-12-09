using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealth : MonoBehaviour
{
    public Slider slider;
    public Camera mainCam;
    public Transform target;
    public Vector3 offset;

    private void Awake()
    {
        mainCam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = mainCam.transform.rotation;
        transform.position = target.position + offset;
    }

    public void UpdateHealthBar(float current, float max)
    {
        slider.value = current / max;
    }
}
