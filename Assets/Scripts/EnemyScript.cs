using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private GameObject[] explodeParticles;
    
    [SerializeField] private float speed = 0.25f;
    [SerializeField] private int life = 1;
    
    private AudioSource audioSource;
    
    private void Awake()
    {
        // DEFINE O JOGADOR COMO SEU OBJETIVO DE PERSEGUIÇÃO
        target = GameObject.Find("Player");
        
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clips[0];
        audioSource.volume = 1f;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void Update()
    {
        // MOVIMENTAÇÃO DO INIMIGO
        transform.position = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);

        // CORRIGE A ALTURA DO INIMIGO NO EIXO Y
        Vector3 point = target.transform.position;
        point.y = transform.position.y;
        
        // ORIENTA A DIREÇÃO DO INIMIGO PARA O JOGADOR
        transform.LookAt(point);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            life--;

            GameObject.Find("Canvas").GetComponent<DataScript>().score++;
            GameObject.Find("Canvas").GetComponent<UIScript>().UIRefresh();

            Destroy(collision.gameObject);
            
            if (life <= 0)
            {
                DestroyEnemy();
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        audioSource.clip = clips[1];
        audioSource.volume = 1f;
        audioSource.loop = false;
        audioSource.Play();

        for (int i = 0; i < explodeParticles.Length; i++)
        {
            GameObject explode = Instantiate(explodeParticles[i]);
            explode.transform.position = transform.position;
            Destroy(explode, 1f);
        }
                
        Destroy(this.gameObject, 0.25f);
    }
}
