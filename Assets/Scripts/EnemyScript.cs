using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float updateRate = 0.5f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float stopDistance = 1.5f;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform playerTransform;
    private ImpactController impactController;

    private float nextUpdateTime = 0f;
    private float attackCooldown = 0f;

    private bool IsOnRange = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        impactController = GetComponentInChildren<ImpactController>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found by EnemyAI.");
        }
    }

    private void OnEnable()
    {
        impactController.OnImpact += HandleImpact;
    }

    private void OnDisable()
    {
        impactController.OnImpact -= HandleImpact;
    }



    private void Update()
    {
        HandleMovement();
        HandleAttackCooldown();
        HandleAnimations();

        if (IsOnRange && playerTransform != null)
        {
            AttemptAttack(playerTransform.gameObject);
        }

    }


    private void HandleAnimations()
    {
        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        animator.SetFloat("speed", ((agent.velocity.magnitude - 0) / (agent.speed - 0)));

    }

    private void HandleMovement()
    {
        if (playerTransform == null || !agent.enabled) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > stopDistance)
        {
            if (Time.time >= nextUpdateTime)
            {
                nextUpdateTime = Time.time + updateRate;
                agent.SetDestination(playerTransform.position);
            }
        }
        else
        {

            agent.ResetPath(); // Se detiene al estar lo suficientemente cerca
        }
    }

    private void HandleAttackCooldown()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.ResetPath();
            IsOnRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerTransform != null)
        {
            IsOnRange = false;
            agent.SetDestination(playerTransform.position);
        }
    }

    private void HandleImpact(Impact impact)
    {
        //Handle Impact Logic Here ->
    }

    private void AttemptAttack(GameObject target)
    {
        if (attackCooldown <= 0f)
        {
            PerformAttack(target);
            attackCooldown = attackDelay;
        }
    }

    private void PerformAttack(GameObject target)
    {
        // Animations
        animator.SetTrigger("attack");
        Debug.Log($"Enemy attacks {target.name}!");
    }
}