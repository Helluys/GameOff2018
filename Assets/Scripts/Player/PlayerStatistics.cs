﻿using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatistics", menuName = "Game data/Player Statistics")]
public class PlayerStatistics : ScriptableObject {

    private const float MAX_ANGULAR_VELOCITY = 100f;
    
    public float maxHealth;
    public float maxStamina;

    public float acceleration;
    public float drag;
    public float angularAcceleration;
    public float angularDrag;
    public float rollStrength;

    public float staticFriction;
    public float dynamicFriction;
    public float bounciness;

    public float attackCooldown;

    [System.Serializable]
    public class Instance {

        public float health;
        public float stamina;

        public Instance (PlayerStatistics origin) {
            health = origin.maxHealth;
            stamina = origin.maxStamina;
        }
    }

    public void ApplyStatistics (Player player) {
        Rigidbody rigidbody = player.GetComponent<Rigidbody>();
        rigidbody.drag = drag;
        rigidbody.angularDrag = angularDrag;
        rigidbody.maxAngularVelocity = MAX_ANGULAR_VELOCITY;

        player.GetComponent<Collider>().material = new PhysicMaterial {
            staticFriction = staticFriction,
            dynamicFriction = dynamicFriction,
            bounciness = bounciness
        };

        player.instanceStatistics.health = maxHealth;
        player.instanceStatistics.stamina = maxStamina;
    }
}
