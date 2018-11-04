using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour {

    public EnemyStatistics sharedStatistics { get { return this._sharedStatistics; } }
    public EnemyStatistics.Instance instanceStatistics { get { return this._instanceStatistics; } }

    [SerializeField] private EnemyBehaviour movementBehaviour = null;
    [SerializeField] private EnemyBehaviour combatBehaviour = null;

    [SerializeField] private EnemyStatistics _sharedStatistics = null;
    [SerializeField] private EnemyStatistics.Instance _instanceStatistics = null;

    public event System.Action<Enemy> OnDeath;

    private Coroutine movementCoroutine = null;
    private Coroutine combatCoroutine = null;

    private void Start () {
        this.sharedStatistics.ApplyStatistics(this);

        this.movementBehaviour = Instantiate(this.movementBehaviour);
        this.movementBehaviour.OnStart(this);
        this.movementCoroutine = StartCoroutine(this.movementBehaviour.Run());

        this.combatBehaviour = Instantiate(this.combatBehaviour);
        this.combatBehaviour.OnStart(this);
        this.combatCoroutine = StartCoroutine(this.combatBehaviour.Run());

        this.OnDeath += Enemy_OnDeath;
    }

    public void Damage (float amount) {
        if (amount > this.instanceStatistics.health && OnDeath != null) {
            OnDeath(this);
        }

        this.instanceStatistics.health = Mathf.Max(this.instanceStatistics.health - amount, 0f);
    }

    public void Heal (float amount) {
        this.instanceStatistics.health = Mathf.Min(this.instanceStatistics.health + amount, this.sharedStatistics.maxHealth);
    }

    private void Enemy_OnDeath (Enemy origin) {
        Destroy(this.gameObject);
    }
}
