using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerCombat {

    [SerializeField] private WeakAttack weakAttack;
    [SerializeField] private StrongAttack strongAttack;
    public bool aimBot = false;


    private Player player;
    private SequencePlayer sequencePlayer;

    public void OnStart (Player player) {

        this.player = player;
        sequencePlayer = player.GetComponent<SequencePlayer>();
        sequencePlayer.OnSuccess += OnSequenceSuccess;
        sequencePlayer.OnValidKeyPress += OnValidKeyPress;
    }

    private void OnValidKeyPress (int keyIndex, bool last) {
        if (last)
            return;

        WeakAttack wa = Object.Instantiate(weakAttack,player.transform.position,Quaternion.identity);
        Vector3 direction = Vector3.zero;
        switch (keyIndex)
        {
            case 0:
                direction = Vector3.forward;
                break;
            case 1:
                direction = Vector3.right;
                break;
            case 2:
                direction = Vector3.left;
                break;
            case 3:
                direction = Vector3.back;
                break;
        }

        wa.Init(aimBot, direction);
    }

    private void OnSequenceSuccess()
    {
        StrongAttack sa = Object.Instantiate(strongAttack, player.transform.position, Quaternion.identity);
    }

}
