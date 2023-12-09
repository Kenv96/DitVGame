using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{
    int damage = 3;
    bool growing;
    Vector3 initialScale = new Vector3(0.1f, 0.1f, 0.1f);
    Vector3 growRateScale = new Vector3(0, 8, 0);
    Vector3 growRatePos = new Vector3(0, 0, 8);
    Vector3 startingPoint = new Vector3(0, 1.7f, 0.5f);
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(growing)
        {
            if(transform.localScale.y < 7.99)
            {
                transform.localScale += growRateScale * Time.deltaTime * 3;
                transform.localPosition += growRatePos * Time.deltaTime * 3;
            }
            else
            {
                growing = false;
            }
                
        }
    }

    private void OnEnable()
    {
        if(transform.localScale.y < 7.99)
        {
            growing = true;
        }
    }

    private void OnDisable()
    {
        transform.localScale = initialScale ;
        transform.localPosition = startingPoint ;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
