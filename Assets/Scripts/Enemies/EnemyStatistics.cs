﻿using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "EnemyStatistics", menuName = "Game data/Enemy Statistics")]
public class EnemyStatistics : ScriptableObject {

    private const float MAX_ANGULAR_VELOCITY = 100f;

    public float maxHealth;

    public float acceleration;
    public float speed;
    public float angularAcceleration;
    public float stoppingDistance;

    public float attackDamage;
    public float attackRange;

    [System.Serializable]
    public class Instance {
        public float health;

        public Instance (EnemyStatistics origin) {
            Reset(origin);
        }

        public void Reset (EnemyStatistics sharedStatistics) {
            health = sharedStatistics.maxHealth;
        }
    }

    public void ApplyStatistics (Enemy enemy) {
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent) {
            agent.acceleration = acceleration;
            agent.speed = speed;
            agent.angularSpeed = angularAcceleration;
            agent.stoppingDistance = stoppingDistance;
        }
    }
}
