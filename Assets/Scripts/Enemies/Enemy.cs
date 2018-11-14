using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour {

    public EnemyStatistics sharedStatistics { get { return _sharedStatistics; } }
    public EnemyStatistics.Instance instanceStatistics { get { return _instanceStatistics; } }

    [SerializeField] private EnemyBehaviour movementBehaviour = null;
    [SerializeField] private EnemyBehaviour combatBehaviour = null;

    [SerializeField] private EnemyStatistics _sharedStatistics = null;
    [SerializeField] private EnemyStatistics.Instance _instanceStatistics = null;
    [SerializeField] private ParticleSystem hitEffect;

    public NavMeshAgent agent { get; private set; }
    public new Rigidbody rigidbody { get; private set; }
    public AnimationManager animationManager { get; private set; }
    public LayerMask terrainMask;

    [SerializeField] private bool resetInstanceStatisticsOnStart = true;

    public event System.Action<Enemy> OnDeath;

    private Coroutine movementCoroutine = null;
    private Coroutine combatCoroutine = null;

    private void Start () {
        if (resetInstanceStatisticsOnStart) {
            sharedStatistics.ApplyStatistics(this);
        }
        agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        animationManager = transform.Find("Graphics").GetComponent<AnimationManager>();

        movementBehaviour = Instantiate(movementBehaviour);
        movementBehaviour.OnStart(this);
        movementCoroutine = StartCoroutine(movementBehaviour.Run());

        combatBehaviour = Instantiate(combatBehaviour);
        combatBehaviour.OnStart(this);
        combatCoroutine = StartCoroutine(combatBehaviour.Run());

        GameManager.instance.AddMonster();
        OnDeath += Enemy_OnDeath;
    }

    private void Update () {
        if (!agent.enabled && IsGrounded()) {
            rigidbody.velocity = Vector3.zero;
            rigidbody.isKinematic = true;
            agent.enabled = true;
        }
    }

    private bool IsGrounded () {
        RaycastHit info;
        if (Physics.Raycast(transform.position, Vector3.down, out info, 5, terrainMask)) {
            return info.distance < 1;
        }
        return false;
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Attack") {
            other.gameObject.GetComponent<Attack>().OnEnter(this);
            hitEffect.Play(true);
        }
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
        GameManager.instance.RemoveMonster();
        Destroy(gameObject);
    }
}
