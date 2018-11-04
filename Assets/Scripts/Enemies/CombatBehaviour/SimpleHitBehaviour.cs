using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleHitBehaviour", menuName = "Game data/AI/Combat/Simple hit")]
public class SimpleHitBehaviour : AIBehaviour {

    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float attackRange = 1f;

    private GameObject gameobject;
    private Player player;
    private WaitForSeconds waitForCooldown;
    private WaitUntil waitInRange;

    public override void OnStart (GameObject gameobject) {
        this.gameobject = gameobject;
        this.player = GameManager.instance.GetPlayer();

        this.waitForCooldown = new WaitForSeconds(this.attackCooldown);
        this.waitInRange = new WaitUntil(() => (this.gameobject.transform.position - this.player.transform.position).magnitude < this.attackRange);
    }

    public override IEnumerator Run () {
        while (true) {
            yield return this.waitInRange;
            this.player.Damage(this.damage);
            yield return this.waitForCooldown;
        }
    }
}
