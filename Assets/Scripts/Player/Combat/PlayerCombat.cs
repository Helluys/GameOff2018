﻿using UnityEngine;

[System.Serializable]
public class PlayerCombat {

    [SerializeField] private WeakAttack weakAttack;
    [SerializeField] private StrongAttack strongAttack;
    [SerializeField] private Shield shield;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color rageColor;
    [SerializeField] private Color shieldColor;

    private Color color;
    public bool aimBot = false;

    private Player player;
    private SequencePlayer sequencePlayer;

    private float defaultStrongAttackSize = 20;
    private float defaultStrongAttackDamage = 50;
    private float defaultWeakAttackDamage = 10;

    public float strongAttackSize = 20;
    public float strongAttackDamage = 50;
    public float weakAttackDamage = 10;

    public void OnStart (Player player) {
        color = normalColor;
        this.player = player;
        sequencePlayer = player.GetComponent<SequencePlayer>();
        sequencePlayer.OnSuccess += OnSequenceSuccess;
        sequencePlayer.OnValidKeyPress += OnValidKeyPress;
    }

    private void OnValidKeyPress (InputType input, bool last) {
        if (last)
            return;

        WeakAttack wa = Object.Instantiate(weakAttack, player.transform.position, Quaternion.identity);
        Vector3 direction = Vector3.zero;

        switch (input)
        {
            case InputType.Green:
                direction = (Vector3.forward + Vector3.right).normalized;
                break;
            case InputType.Red:
                direction = (Vector3.right + Vector3.back).normalized;
                break;
            case InputType.Yellow:
                direction = (Vector3.left + Vector3.forward).normalized;
                break;
            case InputType.Blue:
                direction = (Vector3.back + Vector3.left).normalized;
                break;
        }
        wa.Init(weakAttackDamage,aimBot, direction,color);
    }

    private void OnSequenceSuccess () {
        StrongAttack sa = Object.Instantiate(strongAttack, player.transform.position, Quaternion.identity);
        sa.Init(strongAttackDamage,strongAttackSize,color);
        ResetAttackStats();
    }

    public void DamageUp(float damageMultiplier)
    {
        color = rageColor;
        strongAttackDamage *= damageMultiplier;
        strongAttackSize *= damageMultiplier;
        weakAttackDamage *= damageMultiplier;
    }

    private void ResetAttackStats()
    {
        strongAttackDamage = defaultStrongAttackDamage;
        strongAttackSize = defaultStrongAttackSize;
        weakAttackDamage = defaultWeakAttackDamage;
        color = normalColor;
    }

    public void InstantiateShield(float radius,float duration)
    {
        Shield newShield = Object.Instantiate(shield, player.transform.position,Quaternion.identity);
        newShield.Init(radius, duration,shieldColor);
    }
}
