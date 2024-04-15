using UnityEngine;

public class HeroScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject mesh;
    [SerializeField] private Camera heroCamera;
    
    private Rigidbody rb;
    private Collider collider;
    private Animation anim;

    private Vector3 pointToGo;
    
    private void Start()
    {
        pointToGo = transform.position;
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        anim = GetComponent<Animation>();
    }

    private void Update()
    {
        Touch();
        Move();
    }

    private void Touch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Ray ray = heroCamera.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Ground"))
                    {
                        pointToGo = hit.point;
                        pointToGo.y = transform.position.y;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                pointToGo = transform.position;
            }
        }
    }

    private void Move()
    {
        float distance = Vector3.Distance(transform.position, pointToGo);
        
        if ((int)distance != 0)
        {
            // ESTÁ INDO PARA NOVO DESTINO
            transform.position = Vector3.MoveTowards(transform.position, pointToGo, speed * Time.deltaTime);
            transform.LookAt(pointToGo);
            anim.CrossFade("Run");
        }
        else
        {
            // ESTÁ PARADO NO DESTINO
            anim.CrossFade("Idle");
        }
    }
}
