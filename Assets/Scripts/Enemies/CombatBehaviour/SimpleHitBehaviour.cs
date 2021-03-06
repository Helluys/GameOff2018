﻿using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleHitBehaviour", menuName = "Game data/AI/Combat/Simple hit")]
public class SimpleHitBehaviour : EnemyBehaviour {

    private Enemy enemy;
    private Player player;

    private WaitUntil waitForCooldown;
    private WaitUntil waitAttack;

    private bool isAttacking;

    public override void OnStart (Enemy enemy) {
        this.enemy = enemy;
        player = GameManager.instance.GetPlayer();

        waitForCooldown = new WaitUntil(() => !isAttacking);
        waitAttack = new WaitUntil(() => !isAttacking && IsPlayerInRange());

        enemy.animationManager.OnAnimationEvent += AnimationListener;
    }

    public override IEnumerator Run () {
        while (true) {
            yield return waitAttack;
            isAttacking = true;
            enemy.animationManager.animator.SetTrigger("attack");
            yield return waitForCooldown;
        }
    }

    private void AnimationListener (string eventName) {
        if (eventName.Equals("attackEvent")) {
            isAttacking = false;
            if (!player.state.isRolling && IsPlayerInRange() && !player.state.isBlindToDamage) {
                player.Damage(enemy.sharedStatistics.attackDamage);
            }
        }
    }

    private bool IsPlayerInRange () {
        return (enemy.transform.position - player.transform.position).magnitude < enemy.sharedStatistics.attackRange;
    }
}
