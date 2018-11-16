using UnityEngine;

public class ExitPortal : MonoBehaviour {

    private bool opened = false;
    private Animator animator;

    private void Start () {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter (Collider other) {
        if (other.tag.Equals("Player") && !opened) {
            GameManager.instance.GetPlayer().sequenceManager.player.OnSuccess += SequencePlayer_OnSuccess;
        }
    }

    private void OnTriggerExit (Collider other) {
        if (other.tag.Equals("Player") && !opened) {
            GameManager.instance.GetPlayer().sequenceManager.player.OnSuccess -= SequencePlayer_OnSuccess;
        }
    }

    private void OnTriggerStay (Collider other) {
        if (other.tag.Equals("Player") && opened) {
            Enter();
        }
    }

    private void SequencePlayer_OnSuccess () {
        Open();
    }

    private void Open () {
        opened = true;
        animator.SetTrigger("Open");
        GameManager.instance.GetPlayer().sequenceManager.player.OnSuccess -= SequencePlayer_OnSuccess;
    }

    private void Enter () {
        animator.SetTrigger("Enter");
        GameManager.instance.EndLevel();
    }
}
