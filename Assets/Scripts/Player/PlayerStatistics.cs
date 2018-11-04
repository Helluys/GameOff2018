﻿using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatistics", menuName = "Game data/Player Statistics")]
public class PlayerStatistics : ScriptableObject {

    private const float MAX_ANGULAR_VELOCITY = 100f;

    public float maxHealth;

    public float acceleration;
    public float drag;
    public float angularAcceleration;
    public float angularDrag;

    public float staticFriction;
    public float dynamicFriction;
    public float bounciness;

    public float attackCooldown;

    [System.Serializable]
    public class Instance {

        public float health;

        public Instance (PlayerStatistics origin) {
            this.health = origin.maxHealth;
        }
    }

    public void ApplyStatistics (Player player) {
        Rigidbody rigidbody = player.GetComponent<Rigidbody>();
        rigidbody.drag = this.drag;
        rigidbody.angularDrag = this.angularDrag;
        rigidbody.maxAngularVelocity = MAX_ANGULAR_VELOCITY;

        player.GetComponent<Collider>().material = new PhysicMaterial {
            staticFriction = this.staticFriction,
            dynamicFriction = this.dynamicFriction,
            bounciness = this.bounciness
        };

        player.instanceStatistics.health = this.maxHealth;
    }
}
