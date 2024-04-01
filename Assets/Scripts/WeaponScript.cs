using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;

    public void Fire()
    {
        Instantiate(projectilePrefab, transform.position, transform.rotation);
    }
}
