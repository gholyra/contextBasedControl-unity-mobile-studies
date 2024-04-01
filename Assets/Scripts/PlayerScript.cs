using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float waitTimeForFire;
    
    private Rigidbody rb;
    private string hitTag;
    private bool isFiring;

    private Touch touch;
    private Ray ray;
    private RaycastHit hit;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ActionTouch();
    }

    private void ActionTouch()
    {
        // VERIFICA SE HÁ TOQUE NA TELA (SINGLE TOUCH)
        if (Input.touchCount > 0)
        {
            // ARMAZENA O TOQUE PARA VERIFICAR COMPORTAMENTO
            touch = Input.GetTouch(0);

            // AVALIA O COMPORTAMENTO DO TOQUE
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                // A PARTIR DO TOQUE, CRIA UM RAIO (XY) DA TELA E LANÇA PARA O AMBIENTE 3D
                ray = playerCamera.ScreenPointToRay(touch.position);
                
                // VERIFICA ONDE O RAIO COLIDIU (ESSAS INFORMAÇÕES FICARAM NA VARIÁVEL HIT)
                if (Physics.Raycast(ray, out hit))
                {
                    //AÇÕES DE CONTEXTO
                    if (hit.collider.CompareTag("Ground"))
                    {
                        // MOVIMENTO
                        Vector3 point = hit.point;
                        point.y = transform.position.y;
                        transform.position = Vector3.Lerp(transform.position, point, speed * Time.deltaTime);
                        
                        // ORIENTAÇÃO DO MOVIMENTO
                        transform.LookAt(point);
                    }
                    else if (hit.collider.CompareTag("Enemy") && !isFiring)
                    {
                        // ATAQUE (ATIRAR)
                        Vector3 enemyPoint = hit.collider.transform.position;
                        enemyPoint.y = transform.position.y;
                        transform.LookAt(enemyPoint);
                        StartCoroutine(Firing());
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                hitTag = null;
            }
        }
    }

    private IEnumerator Firing()
    {
        isFiring = true;
        GameObject.Find("Pointer").GetComponent<WeaponScript>().Fire();
        yield return new WaitForSeconds(waitTimeForFire);
        isFiring = false;
    }
}
