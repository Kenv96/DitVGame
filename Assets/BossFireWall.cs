using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireWall : MonoBehaviour
{
    // Update is called once per frame
    private void Start()
    {
        Destroy(gameObject, 10);
        transform.eulerAngles = new Vector3(0, 160, 0);
    }

    void Update()
    {
        transform.eulerAngles += new Vector3(0, -13f, 0) * Time.deltaTime;
        //gameObject.transform.Rotate(new Vector3(0,0,-0.001f), Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

}
