using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    public PlayerStatistics sharedStatistics { get { return _sharedStatistics; } }
    public PlayerStatistics.Instance instanceStatistics { get { return _instanceStatistics; } }
    public AnimationManager animationManager { get; private set; }
    public PlayerState state { get; private set; }
    public SequenceManager sequenceManager;
    public HUDManager hud;
    public bool alive { get; private set; }

    [SerializeField] private PlayerStatistics _sharedStatistics = null;
    [SerializeField] private PlayerStatistics.Instance _instanceStatistics = null;

    [SerializeField] private PlayerMovement movement = null;
    public PlayerItems items = null;
    public PlayerCombat combat = null;

    public event System.Action<Player> OnDeath;

    private void Start () {
        animationManager = transform.Find("Graphics").GetComponent<AnimationManager>();
        state = new PlayerState();

        sharedStatistics.ApplyStatistics(this);

        movement.OnStart(this);
        combat.OnStart(this);
        items.OnStart(this);

        OnDeath += Player_OnDeath;
        alive = true;
    }

    private void Update () {
        if (alive) {
            movement.OnUpdate();
            items.OnUpdate();
        }
    }

    public void Damage (float amount) {
        if (alive && amount > instanceStatistics.health && OnDeath != null) {
            OnDeath(this);
        }
        instanceStatistics.health = Mathf.Max(instanceStatistics.health - amount, 0f);
        hud.UpdateHealtBar(instanceStatistics.health / sharedStatistics.maxHealth);
        hud.HitEffect();
    }

    public void Heal (float amount) {
        instanceStatistics.health = Mathf.Min(instanceStatistics.health + amount, sharedStatistics.maxHealth);
    }

    public void StaminaRegen (float amount) {
        instanceStatistics.stamina = Mathf.Min(instanceStatistics.stamina + amount, sharedStatistics.maxStamina);
    }

    private void Player_OnDeath (Player origin) {
        animationManager.animator.SetTrigger("die");
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        alive = false;
    }
}
