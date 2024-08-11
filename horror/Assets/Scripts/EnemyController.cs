using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public Transform player;
    public float chaseSpeed;
    public float maxChaseSpeed;
    public float acceleration;
    public float sightDistance;
    public float catchDistance;
    public float jumpscareTime;
    public string deathScene;
    public float teleportTime = 10f;

    public AudioClip chaseSound;
    public AudioClip catchSound;

    private AudioSource audioSource;
    private bool isChasing = false;
    private float chaseTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 0f;
        agent.stoppingDistance = 0.1f;

        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        if (!GetComponent<Collider>())
        {
            gameObject.AddComponent<CapsuleCollider>().isTrigger = true;
        }
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            StartChase();
        }

        if (isChasing)
        {
            chaseTimer += Time.deltaTime;

            if (chaseTimer >= teleportTime)
            {
                TeleportAndCatchPlayer();
            }
            else
            {
                agent.SetDestination(player.position);

                if (Vector3.Distance(transform.position, player.position) <= catchDistance)
                {
                    TeleportAndCatchPlayer();
                }

                if (agent.velocity.sqrMagnitude > 0.1f)
                {
                    animator.SetFloat("Speed", agent.velocity.magnitude);
                }
                else
                {
                    animator.SetFloat("Speed", 0f);
                }

                if (agent.speed < maxChaseSpeed)
                {
                    agent.speed += acceleration * Time.deltaTime;
                    if (agent.speed > maxChaseSpeed)
                    {
                        agent.speed = maxChaseSpeed;
                    }
                }
            }
        }
        else
        {
            agent.SetDestination(transform.position);
            animator.ResetTrigger("run");
            animator.SetTrigger("idle");
            agent.speed = 0f;
            chaseTimer = 0f;
        }
    }

    void StartChase()
    {
        if (!isChasing)
        {
            Debug.Log("Chase started");
            isChasing = true;
            agent.speed = chaseSpeed;
            animator.ResetTrigger("idle");
            animator.SetTrigger("run");
            PlaySound(chaseSound);
        }
    }

    void StartJumpscare()
    {
        Debug.Log("Starting jumpscare");
        //player.gameObject.SetActive(false);
        animator.ResetTrigger("run");
        animator.SetTrigger("jumpscare");
        PlaySound(catchSound);
        isChasing = false;
        agent.isStopped = true; // Stop the NavMeshAgent
        agent.speed = 0f;
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        Debug.Log("Player caught, starting death routine");
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene(deathScene);
    }

    bool CanSeePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, sightDistance))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player seen");
                return true;
            }
        }

        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            TeleportAndCatchPlayer();
        }
    }

    void TeleportAndCatchPlayer()
    {
        Debug.Log("Teleporting to player");
        transform.position = player.position;
        StartJumpscare();
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
