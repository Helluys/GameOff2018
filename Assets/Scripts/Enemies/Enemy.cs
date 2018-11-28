using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public EnemyStatistics sharedStatistics { get { return _sharedStatistics; } }
    public EnemyStatistics.Instance instanceStatistics { get { return _instanceStatistics; } }

    [SerializeField] private List<EnemyBehaviour> behaviours = new List<EnemyBehaviour>();

    [SerializeField] private EnemyStatistics _sharedStatistics = null;
    [SerializeField] private EnemyStatistics.Instance _instanceStatistics = null;
    [SerializeField] private ParticleSystem hitEffect;

    public new Rigidbody rigidbody { get; private set; }
    public AnimationManager animationManager { get; private set; }

    [SerializeField] private bool resetInstanceStatisticsOnStart = true;

    public event System.Action<Enemy> OnDeath;
    public event System.Action<Enemy, float> OnDamage;

    private void Start () {
        if (resetInstanceStatisticsOnStart) {
            sharedStatistics.ApplyStatistics(this);
            _instanceStatistics.Reset(sharedStatistics);
        }
        rigidbody = GetComponent<Rigidbody>();
        animationManager = transform.Find("Graphics").GetComponent<AnimationManager>();

        List<EnemyBehaviour> instantiatedBehaviours = new List<EnemyBehaviour>();
        for (int i = 0; i < behaviours.Count; i++) {
            EnemyBehaviour behaviour = Instantiate(behaviours[i]);
            instantiatedBehaviours.Add(behaviour);
            behaviour.OnStart(this);
            StartCoroutine(behaviour.Run());
        }
        behaviours = instantiatedBehaviours;

        GameManager.instance.AddEnemy(this);
        OnDeath += Enemy_OnDeath;
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Attack") {
            other.gameObject.GetComponent<Attack>().OnEnter(this);
            hitEffect.Play(true);
        }
    }

    public void Damage (float amount) {
        if(OnDamage != null) {
            OnDamage(this, amount);
        }

        if (amount > instanceStatistics.health && OnDeath != null) {
            OnDeath(this);
        }

        instanceStatistics.health = Mathf.Max(instanceStatistics.health - amount, 0f);
    }

    public void Heal (float amount) {
        instanceStatistics.health = Mathf.Min(instanceStatistics.health + amount, sharedStatistics.maxHealth);
    }

    private void Enemy_OnDeath (Enemy origin) {
        GameManager.instance.RemoveEnemy(this);
        Destroy(gameObject);
    }
}
