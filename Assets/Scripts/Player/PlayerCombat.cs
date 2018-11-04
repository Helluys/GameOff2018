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

        waitForAttack = new WaitUntil(() => attack == true);
        waitForAttackCooldown = new WaitForSeconds(this.player.sharedStatistics.attackCooldown);

        combatCoroutine = player.StartCoroutine(CombatLoop());
    }

    private void SequencePlayer_OnSuccess () {
        attack = true;
    }

    public void OnUpdate () {
        // Nothing to do
    }

    private IEnumerator CombatLoop () {
        while (true) {
            yield return waitForAttack;
            Attack();
            yield return waitForAttackCooldown;
        }
    }

    private void Attack () {
        attack = false;
        Object.Instantiate(attackPrefab, player.transform);
    }
}
