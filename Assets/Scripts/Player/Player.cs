using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    public PlayerStatistics sharedStatistics { get { return this._sharedStatistics; } }
    public PlayerStatistics.Instance instanceStatistics { get { return this._instanceStatistics; } }

    [SerializeField] private PlayerStatistics _sharedStatistics = null;
    [SerializeField] private PlayerStatistics.Instance _instanceStatistics = null;

    [SerializeField] private PlayerMovement movement = null;
    [SerializeField] private PlayerCombat combat = null;

    public event System.Action<Player> OnDeath;

    private void Start () {
        this.sharedStatistics.ApplyStatistics(this);

        this.movement.OnStart(this);
        this.combat.OnStart(this);

        OnDeath += Player_OnDeath;
    }

    private void Update () {
        this.movement.OnUpdate();
        this.combat.OnUpdate();
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

    private void Player_OnDeath (Player origin) {
        Debug.Log("LOL u ded, rrrrise againnn!");
        this.instanceStatistics.health = this.sharedStatistics.maxHealth;
    }
}
