using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleHitBehaviour", menuName = "Game data/AI/Combat/Simple hit")]
public class SimpleHitBehaviour : EnemyBehaviour {

    private Enemy enemy;
    private Player player;

    private WaitForSeconds waitForCooldown;
    private WaitUntil waitInRange;

    public override void OnStart (Enemy enemy) {
        this.enemy = enemy;
        this.player = GameManager.instance.GetPlayer();

        this.waitForCooldown = new WaitForSeconds(this.enemy.sharedStatistics.attackCooldown);
        this.waitInRange = new WaitUntil(() => (this.enemy.transform.position - this.player.transform.position).magnitude < this.enemy.sharedStatistics.attackRange);
    }

    public override IEnumerator Run () {
        while (true) {
            yield return this.waitInRange;
            this.player.Damage(this.enemy.sharedStatistics.attackDamage);
            yield return this.waitForCooldown;
        }
    }
}
