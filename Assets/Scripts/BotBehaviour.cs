using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]

public class BotBehaviour : MonoBehaviour
{
    [SerializeField] private float deletionDelay = 5f;
    [SerializeField] private int maxHp = 100;
    private int hp;

    private float speed = 10f;

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
    }

    private async void StartMovement()
    {
        isMoving = true;
        animator.SetTrigger("Hello");

        await Task.Delay(3100);

        if (animator != null)
            animator.SetBool("Run", true);
        while (isMoving)
        {
            MoveToPlayerDestinationAsync();
            await Task.Delay(200); // Ожидание 0.2 секунды перед обновлением точки назначения
        }
    }

    private void MoveToPlayerDestinationAsync()
    {
        Vector3 destination = player.transform.position;
        agent.SetDestination(destination);
    }

    private async Task CheckDestinationReached(TaskCompletionSource<bool> tcs)
    {
        while (true)
        {
            await Task.Yield();

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                tcs.SetResult(true);
                return;
            }

            if (!isMoving)
            {
                tcs.SetCanceled();
                return;
            }
        }
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
        if (!isDead)
            Die();
    }
}