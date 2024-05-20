using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroScript : MonoBehaviour
{
    [SerializeField] private GameObject[] meshes;
    
    [SerializeField] private float speed;
    [SerializeField] private GameObject mesh;
    [SerializeField] private Camera heroCamera;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float waitTimeForFire;
    [SerializeField] private Light flashLight;
    [SerializeField] private GameObject sparkParticles;
    [SerializeField] private Transform sparkPoint;
    [SerializeField] private int invincibleTime;
    
    private Rigidbody rb;
    private Collider collider;
    private Animation anim;
    private AudioSource audioSource;
    
    private Vector3 pointToGo;
    private bool isRunning;

    [SerializeField] private float timingIsHurt;
    private bool isHurt;

    private bool isFiring;
    
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
        Fire();
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
                        
                        isFiring = false;
                    }
                    else if (hit.collider.CompareTag("Enemy") && !isFiring)
                    {
                        Vector3 enemyPosition = hit.point;
                        enemyPosition.y = transform.position.y;
                        StartCoroutine(Firing(enemyPosition));
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
        if (!isHurt && !isFiring)
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
    }

    private void Audios()
    {
        if (isRunning && !audioSource.isPlaying && !isHurt)
        {
            audioSource.clip = audioClips[0];
            audioSource.volume = 0.3f;
            audioSource.pitch = 0.65f;
            audioSource.Play();
        }
        else if (!isRunning && !isHurt)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            StartCoroutine(Hurt(timingIsHurt));
        }
    }

    private IEnumerator Hurt(float t)
    {
        DamageData(1);
        
        isHurt = true;

        audioSource.clip = audioClips[1];
        audioSource.volume = 1f;
        audioSource.pitch = 1f;
        audioSource.Play();
            
        anim.CrossFade("Hurt");
        
        yield return new WaitForSeconds(t);

        isHurt = false;
    }

    private void Fire()
    {
        if (isFiring)
        {
            anim.CrossFade("Fire");
        }
    }
    
    private IEnumerator Firing(Vector3 position)
    {
        isFiring = true;
        transform.LookAt(position);
        GameObject.Find("Pointer").GetComponent<WeaponScript>().Fire();

        StartCoroutine(FlashLight());
        
        yield return new WaitForSeconds(waitTimeForFire);
        
        isFiring = false;
    }

    private IEnumerator FlashLight()
    {
        flashLight.enabled = true;

        GameObject spark = Instantiate(sparkParticles);
        spark.transform.position = sparkPoint.position;
        Destroy(spark, 0.3f);
        
        yield return new WaitForSeconds(0.1f);
        flashLight.enabled = false;
    }

    private void DamageData(int damagePoints)
    {
        GameObject.Find("Canvas").GetComponent<UIScript>().UIRefresh();
        GameObject.Find("Canvas").GetComponent<DataScript>().hp -= damagePoints;

        if (GameObject.Find("Canvas").GetComponent<DataScript>().hp <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
        else
        {
            StartCoroutine(Invincible(this.invincibleTime));
        }
    }

    private IEnumerator Invincible(int time)
    {
        collider.enabled = false;
        for (int i = 0; i < time; i++)
        {
            meshes[0].GetComponent<MeshRenderer>().enabled = !meshes[0].GetComponent<MeshRenderer>().enabled;
            meshes[1].GetComponent<MeshRenderer>().enabled = !meshes[1].GetComponent<MeshRenderer>().enabled;
            meshes[2].GetComponent<SkinnedMeshRenderer>().enabled = !meshes[2].GetComponent<SkinnedMeshRenderer>().enabled;
                
            yield return new WaitForSeconds(1f);
        }
        meshes[0].GetComponent<MeshRenderer>().enabled = true;
        meshes[1].GetComponent<MeshRenderer>().enabled = true;
        meshes[2].GetComponent<SkinnedMeshRenderer>().enabled = true;
            
        collider.enabled = true;
    }
}
