using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;

    [Header("Detection")]
    public float wanderRadius = 10f;
    public float detectionRange = 15f;
    public float attackRange = 2f;
    public float fieldOfView = 110f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private float attackTimer;
    private bool isDead = false;
    private bool canSeePlayer = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        InvokeRepeating("Wander", 0f, 5f);
    }

    private void Update()
    {
        if (!canSeePlayer && agent.remainingDistance < 0.5f)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        if (isDead) return;

        canSeePlayer = CanSeePlayer();
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        attackTimer += Time.deltaTime;

        if (canSeePlayer && distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else if (canSeePlayer)
        {
            Chase();
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
        }
        float speed = agent.velocity.magnitude;
        animator.SetBool("isWalking", speed > 0.1f && !canSeePlayer);
        animator.SetBool("isRunning", speed > 0.1f && canSeePlayer);
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;

        if (distance > detectionRange) return false;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > fieldOfView / 2f) return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer.normalized, out hit, detectionRange))
        {
            if (hit.transform.CompareTag("Player"))
                return true;
        }

        return false;
    }

    void Chase()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }

    void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        animator.SetBool("isAttacking", true);
        animator.SetBool("isRunning", false);

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            PlayerState.Instance.TakeDamage(attackDamage);
        }
    }

    void Wander()
    {
        if (isDead || canSeePlayer) return;

        Vector3 randomPoint = transform.position + Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        health -= damage;
        if (health <= 0f) Die();
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("isDead");
        Destroy(gameObject, 3f);
    }
}