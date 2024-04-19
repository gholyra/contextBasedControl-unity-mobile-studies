using UnityEngine;

public class HeroScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject mesh;
    [SerializeField] private Camera heroCamera;
    [SerializeField] private AudioClip[] audioClips;
    
    private Rigidbody rb;
    private Collider collider;
    private Animation anim;
    private AudioSource audioSource;
    
    private bool isRunning;
    private Vector3 pointToGo;
    
    private void Start()
    {
        pointToGo = transform.position;
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        anim = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Touch();
        Move();
        Audios();
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
            isRunning = true;
        }
        else
        {
            // ESTÁ PARADO NO DESTINO
            anim.CrossFade("Idle");
            isRunning = false;
        }
    }

    private void Audios()
    {
        if (isRunning && !audioSource.isPlaying)
        {
            audioSource.clip = audioClips[0];
            audioSource.volume = 0.5f;
            audioSource.pitch = 0.85f;
            audioSource.Play();
        }
        else if (!isRunning)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
    }
}
