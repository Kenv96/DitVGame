using UnityEngine;

public class SpellOrbShoot : MonoBehaviour
{
    public GameObject orbshot;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Vector3 targetPoint = other.transform.position + new Vector3(0,1.5f,0);

            Vector3 directionOfShot = targetPoint - transform.position;

            GameObject currentSpell = Instantiate(orbshot, transform.position, Quaternion.identity);
            currentSpell.transform.forward = directionOfShot.normalized;

            currentSpell.GetComponent<Rigidbody>().AddForce(directionOfShot.normalized * 30, ForceMode.Impulse);

            gameObject.SetActive(false);
        }
    }
}
