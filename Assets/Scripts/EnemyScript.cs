using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private int currentEnemy;
    private float speed;
    private int life;
    
    private void Start()
    {
        // SORTEIA UM NÚMERO PARA DEFINIR A CATEGORIA DO INIMIGO
        // 0 (RÁPIDO E FRACO)
        // 1 (LENTO E FORTE)
        // 2 (MÉDIO E MÉDIO)
        currentEnemy = Random.Range(0, 3);
        
        // ATRIBUI OS VALORES DE ACORDO COM A CATEGORIA
        if (currentEnemy == 0)
        {
            speed = 1f;
            life = 1;
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else if (currentEnemy == 1)
        {
            speed = 0.25f;
            life = 5;
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            speed = 0.5f;
            life = 2;
            GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        
        // DEFINE O JOGADOR COMO SEU OBJETIVO DE PERSEGUIÇÃO
        target = GameObject.FindGameObjectWithTag("Player");
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
            if (life <= 0) Destroy(this.gameObject);
        }
    }
}
