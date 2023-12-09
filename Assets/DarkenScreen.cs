using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DarkenScreen : MonoBehaviour
{
    public Image panel;
    private bool brightening;
    private bool darkening;
    // Start is called before the first frame update
    void Start()
    {
        if(panel.color.a > 0)
        {
            brightening = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(brightening)
        {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - 0.01f);
            if (panel.color.a <= 0.01) brightening = false;
        }
        if (darkening)
        {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a + 0.001f);
            if (panel.color.a >= 0.99) darkening = false;
        }
    }

    public void Darken()
    {
        darkening = true;
    }

    public void Brighten()
    {
        brightening = true;
    }
}
