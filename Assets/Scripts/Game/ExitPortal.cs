using System.Collections;

using UnityEngine;

public class ExitPortal : MonoBehaviour {

    public bool active { get; set; }
    private bool opened = false;

    [SerializeField] private new GameObject particleSystem;

    private void Start () {
        StartCoroutine(AnimateMaterial(GetComponent<MeshRenderer>().materials[1]));
    }

    private IEnumerator AnimateMaterial (Material portalMaterial) {
        Color color = portalMaterial.color;
        color.a = 0f;
        portalMaterial.color = color;

        yield return new WaitUntil(() => active);

        while (true) {
            color.a = 0.3f * Mathf.Sin(3f * Time.time) + (opened ? 0.7f : 0.3f);
            portalMaterial.color = color;
            yield return null;
        }
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
        particleSystem.SetActive(true);
        GameManager.instance.GetPlayer().sequenceManager.player.OnSuccess -= SequencePlayer_OnSuccess;
    }

    private void Enter () {
        GameManager.instance.EndLevel();
    }
}
