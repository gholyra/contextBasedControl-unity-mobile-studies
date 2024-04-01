using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    private float speed;
    
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);

        Vector3 point = target.transform.position;
        point.y = transform.position.y;
        transform.LookAt(point);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(this.gameObject);
        }
    }
}
