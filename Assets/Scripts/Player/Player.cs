using UnityEngine;
using System;
using System.Collections;

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

    [SerializeField] Material mat;
    [SerializeField] private float hitInvicibilityTime = 1.0f;

    public event System.Action<Player> OnDeath;

    private void Start () {
        animationManager = transform.Find("Graphics").GetComponent<AnimationManager>();
        state = new PlayerState();
        state.isBlindToDamage = false;

        sharedStatistics.ApplyStatistics(this);

        movement.OnStart(this);
        combat.OnStart(this);
        items.OnStart(this);

        OnDeath += Player_OnDeath;
        alive = true;
        UpdateMatColor(0);
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
        StartCoroutine(EBlindToDamage());
    }

    private IEnumerator EBlindToDamage()
    {
        SoundController.Instance.PlaySound(SoundName.TimotheHit);
        state.isBlindToDamage = true;
        gameObject.layer = LayerMask.NameToLayer("GhostPlayer");
        LeanTween.value(gameObject, UpdateMatColor, 0, 1, 0.1f).setLoopPingPong();
        yield return new WaitForSeconds(hitInvicibilityTime);
        LeanTween.cancel(gameObject);
        UpdateMatColor(0);
        state.isBlindToDamage = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void UpdateMatColor(float val)
    {
        mat.color = Color.Lerp(Color.white, Color.black, val);
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
