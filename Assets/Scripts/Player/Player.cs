using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    public PlayerStatistics sharedStatistics { get { return _sharedStatistics; } }
    public PlayerStatistics.Instance instanceStatistics { get { return _instanceStatistics; } }
    public AnimationManager animationManager { get; private set; }
    public PlayerState state { get; private set; }

    [SerializeField] private PlayerStatistics _sharedStatistics = null;
    [SerializeField] private PlayerStatistics.Instance _instanceStatistics = null;

    [SerializeField] private PlayerMovement movement = null;
    [SerializeField] private PlayerCombat combat = null;

    public event System.Action<Player> OnDeath;

    private void Start () {
        animationManager = transform.Find("Graphics").GetComponent<AnimationManager>();
        state = new PlayerState();

        sharedStatistics.ApplyStatistics(this);

        movement.OnStart(this);
        combat.OnStart(this);

        OnDeath += Player_OnDeath;
    }

    private void Update () {
        movement.OnUpdate();
    }

    public void Damage (float amount) {
        if (amount > instanceStatistics.health && OnDeath != null) {
            OnDeath(this);
        }
        instanceStatistics.health = Mathf.Max(instanceStatistics.health - amount, 0f);
        HUDManager.Instance.UpdateHealtBar(instanceStatistics.health / sharedStatistics.maxHealth);
        HUDManager.Instance.HitEffect();
    }

    public void Heal (float amount) {
        instanceStatistics.health = Mathf.Min(instanceStatistics.health + amount, sharedStatistics.maxHealth);
    }

    private void Player_OnDeath (Player origin) {
        Debug.Log("Omae wa mou SHINDEIRU!!!");
        instanceStatistics.health = sharedStatistics.maxHealth;
    }
}
