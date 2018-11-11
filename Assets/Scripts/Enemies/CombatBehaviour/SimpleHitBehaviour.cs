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
        player = GameManager.instance.GetPlayer();

        waitForCooldown = new WaitForSeconds(this.enemy.sharedStatistics.attackCooldown);
        waitInRange = new WaitUntil(() => (this.enemy.transform.position - player.transform.position).magnitude < this.enemy.sharedStatistics.attackRange && !player.state.isRolling);
    }

    public override IEnumerator Run () {
        while (true) {
            yield return waitInRange;
            player.Damage(enemy.sharedStatistics.attackDamage);
            yield return waitForCooldown;
        }
    }
}
