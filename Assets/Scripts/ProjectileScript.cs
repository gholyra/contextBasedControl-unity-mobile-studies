using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private float lifeSpan;
    [SerializeField] private float speed;
    [SerializeField] private bool isVisible = true;

    private void Start()
    {
        // CONTROLA SE O PROJÉTIL SERÁ RENDERIZADO OU NÃO
        GetComponent<MeshRenderer>().enabled = isVisible;
        
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
        if (GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<Collider>().enabled = false;
            Destroy(this.gameObject, 0.5f);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
