using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerCombat {

    [SerializeField] private KeyCode attackKey = KeyCode.Mouse0;
    [SerializeField] private GameObject attackPrefab = null;

    private Player player;

    private Coroutine combatCoroutine;
    private WaitUntil waitForAttack;
    private WaitForSeconds waitForAttackCooldown;
    private bool attack;

    public void OnStart (Player player) {
        this.player = player;

        player.GetComponent<SequencePlayer>().OnSuccess += SequencePlayer_OnSuccess;

        this.waitForAttack = new WaitUntil(() => this.attack == true);
        this.waitForAttackCooldown = new WaitForSeconds(this.player.sharedStatistics.attackCooldown);

        this.combatCoroutine = player.StartCoroutine(CombatLoop());
    }

    private void SequencePlayer_OnSuccess () {
        this.attack = true;
    }

    public void OnUpdate () {
        // Nothing to do
    }

    private IEnumerator CombatLoop () {
        while (true) {
            yield return this.waitForAttack;
            Attack();
            yield return this.waitForAttackCooldown;
        }
    }

    private void Attack () {
        this.attack = false;
        Object.Instantiate(attackPrefab, player.transform);
    }
}
