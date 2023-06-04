using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Responsible for movement of enemy
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class BotBehaviour : MonoBehaviour
{
    [SerializeField] private float deletionDelay = 5f;
    [SerializeField] private int maxHp = 100;
    private int hp;

    private float speed = 10f;

    private string helloAnimationName;
    private float helloDelay = 3.5f;
    private NavMeshAgent agent;
    private GameObject player;
    private bool isMoving;
    private Animator animator;
    private bool isDead;
    private TaskCompletionSource<bool> cancelToken;

    private void Start()
    {
        hp = maxHp;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        agent.speed = speed;

        StartMovement();
    }

    public void ApplyParams(EnemyParams data)
    {
        maxHp = data.HealthPoints;
        if (hp > maxHp)
            hp = maxHp;
        speed = data.Speed;
        agent.speed = speed;
        helloAnimationName = data.HelloAnimationName;
        helloDelay = data.HelloDelay;
    }

    private async void StartMovement()
    {
        if (animator != null)
            animator.SetTrigger("Hello");

        await Task.Delay((int)(helloDelay * 1000));
        if (gameObject == null)
            return;

        isMoving = true;

        if (animator != null)
            animator.SetBool("Run", true);

        while (isMoving)
        {
            MoveToPlayerDestinationAsync();
            await Task.Delay(200);
            if (gameObject == null)
                return;
        }
    }

    private void MoveToPlayerDestinationAsync()
    {
        Vector3 destination = player.transform.position;
        agent.SetDestination(destination);
    }

    public void StopMovement()
    {
        isMoving = false;
    }

    private void Die()
    {
        isMoving = false;
        isDead = true;
        EventsBus.Publish(new OnEnemyDeath() { enemy = this });
    }

    public async void DestroyWithDelay(GameObject gameObject, float delayInSeconds)
    {
        await Task.Delay((int)(delayInSeconds * 1000));
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        cancelToken.SetCanceled();

        if (!isDead)
            Die();
    }
}