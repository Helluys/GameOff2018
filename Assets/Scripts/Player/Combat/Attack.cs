using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour {

    public float knockBackStrength = 15;
    protected Vector3 knockBackDirection;
    public float damageAmount = 10;

    public virtual void OnEnter(Enemy enemy) {
        enemy.Damage(damageAmount);
        enemy.agent.enabled = false;
        enemy.rigidbody.isKinematic = false;
        knockBackDirection = (enemy.transform.position - transform.position).normalized;
        knockBackDirection = (3*Vector3.up + knockBackDirection).normalized;
        enemy.rigidbody.AddForce(knockBackDirection * knockBackStrength, ForceMode.VelocityChange);
    }

    public virtual void OnStay(Enemy enemy) { }
    public virtual void OnExit(Enemy enemy) { }
}
