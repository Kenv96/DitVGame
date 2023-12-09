using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public int damage = 3;

    private RaycastHit[] hits;
    public LayerMask lm;
    public Transform firePoint;
    public Transform fireTarget;
    public LineRenderer lr;

    private void Update()
    {
        lr.SetPosition(0, firePoint.transform.position);
        lr.SetPosition(1, fireTarget.transform.position);
        hits = Physics.RaycastAll(firePoint.position, fireTarget.position - firePoint.position, Vector3.Distance(firePoint.position, fireTarget.position), lm, QueryTriggerInteraction.Ignore);
        foreach (RaycastHit hit in hits) 
        {
            if (hit.collider.gameObject.transform.CompareTag("Enemy"))
            {
                Debug.Log(hit.collider.gameObject.transform.name);
                hit.collider.gameObject.GetComponent<IEnemy>().DrainHealth(damage);
            }
        }
    }
}
