using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject spell;

    public float shootForce;

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;

    public int maxMana;
    private int mana;

    bool shooting, readyToShoot, reloading;

    public Camera tpCam;
    public Transform attackPoint;

    public bool allowInvoke = true;

    // Start is called before the first frame update
    private void Awake()
    {
        mana = maxMana;
        readyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }


    private void MyInput()
    {
        shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (readyToShoot && shooting)
        {
            TakeShot();
        }
    }

    private void TakeShot()
    {
        readyToShoot = false;
        mana--;

        Ray ray = tpCam.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        RaycastHit hit;
        
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Debug.Log(targetPoint.ToString());
        Debug.DrawRay(tpCam.transform.position, targetPoint, Color.red, 10);

        Vector3 directionOfShot = targetPoint - attackPoint.position;

        GameObject currentSpell = Instantiate(spell, attackPoint.position, Quaternion.identity);
        currentSpell.transform.forward = directionOfShot.normalized;

        currentSpell.GetComponent<Rigidbody>().AddForce(directionOfShot.normalized * shootForce, ForceMode.Impulse);

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
}
