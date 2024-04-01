using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private float lifeSpan;
    [SerializeField] private float speed;

    private void Start()
    {
        // DETERMINA O TEMPO DE EXISTÊNCIA DO PROJÉTIL NA CENA
        Destroy(this.gameObject, lifeSpan);
    }

    private void Update()
    {
        // MOVE O PROJÉTIL PARA FRENTE
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // DESTRÓI O PROJÉTIL CASO ELE COLIDA
        Destroy(this.gameObject);
    }
}
