using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour {

    public EnemyStatistics sharedStatistics { get { return _sharedStatistics; } }
    public EnemyStatistics.Instance instanceStatistics { get { return _instanceStatistics; } }

    [SerializeField]  EnemyBehaviour movementBehaviour = null;
    [SerializeField] private EnemyBehaviour combatBehaviour = null;

    [SerializeField] private EnemyStatistics _sharedStatistics = null;
    [SerializeField] private EnemyStatistics.Instance _instanceStatistics = null;

    public NavMeshAgent agent { get; private set; }
    public Animator animator { get; private set; }
    public AnimatorTrigger triggerAnimator { get; private set; }

    [SerializeField] private bool resetInstanceStatisticsOnStart = true;

    public event System.Action<Enemy> OnDeath;

    private Coroutine movementCoroutine = null;
    private Coroutine combatCoroutine = null;

    private void Start () {
        if (resetInstanceStatisticsOnStart) {
            sharedStatistics.ApplyStatistics(this);
        }
        agent = GetComponent<NavMeshAgent>();
        animator = transform.Find("Graphics").GetComponent<Animator>();
        triggerAnimator = transform.Find("Graphics").GetComponent<AnimatorTrigger>();

        movementBehaviour = Instantiate(movementBehaviour);
        movementBehaviour.OnStart(this);
        movementCoroutine = StartCoroutine(movementBehaviour.Run());

        combatBehaviour = Instantiate(combatBehaviour);
        combatBehaviour.OnStart(this);
        combatCoroutine = StartCoroutine(combatBehaviour.Run());

        OnDeath += Enemy_OnDeath;
    }

    public void Damage (float amount) {
        if (amount > instanceStatistics.health && OnDeath != null) {
            OnDeath(this);
        }

        instanceStatistics.health = Mathf.Max(instanceStatistics.health - amount, 0f);
    }

    public void Heal (float amount) {
        instanceStatistics.health = Mathf.Min(instanceStatistics.health + amount, sharedStatistics.maxHealth);
    }

    private void Enemy_OnDeath (Enemy origin) {
        Destroy(gameObject);
    }
}
