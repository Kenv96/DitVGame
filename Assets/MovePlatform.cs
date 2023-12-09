using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    private Vector3 startpos;
    private GameObject highlight;
    private bool lowering, raising;
    private void Start()
    {
        startpos = transform.position;
        highlight = transform.Find("Highlight").gameObject;
        highlight.SetActive(false);
    }

    private void Update()
    {
        if(lowering)
        {
            transform.position -= new Vector3 (0,1f,0) * Time.deltaTime ;
            if(transform.position.y < -2)
            {
                lowering = false;
            }
        }
        if (raising)
        {
            transform.position += new Vector3(0, 1f, 0) * Time.deltaTime;
            if (transform.position.y > startpos.y)
            {
                transform.position = new Vector3(transform.position.x, startpos.y, transform.position.z);
                raising = false;
            }
        }
    }

    public void StartLower()
    {
        highlight.SetActive(true);
        Invoke(nameof(LowerPlatform), 4.0f);
        Invoke(nameof(RaisePlatform), 20f);
    }

    private void LowerPlatform()
    {
        lowering = true;
    }

    private void RaisePlatform()
    {
        highlight.SetActive(false);
        raising = true;
    }
}
