using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    [SerializeField] private PlayerStatistics statistics;
    [SerializeField] private PlayerMovement movement;

    [SerializeField]private PlayerStatistics.Instance instanceStatistics;

    public float Health {
        get {
            return this.instanceStatistics.health;
        }

        set {
            this.instanceStatistics.health = value;
        }
    }

    public float MaxHealth {
        get {
            return this.statistics.maxHealth;
        }
    }

    public void Damage(float amount) {
        if (amount > Health)
            OnDeath();

        Health = Mathf.Max(Health - amount, 0f) ;
    }

    public void Heal(float amount) {
        Health = Mathf.Min(Health + amount, this.statistics.maxHealth);
    }

    private void OnDeath () {
        Debug.Log("LOL u ded");
    }

    private void Start () {
        this.statistics.ApplyStatistics(this);
        this.movement.OnStart(GetComponent<Rigidbody>());
    }

    private void Update () {
        this.movement.OnUpdate();
    }
}
