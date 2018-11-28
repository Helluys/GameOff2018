using System;
using System.Collections;

using UnityEngine;

public class ExitPortal : MonoBehaviour {

    public event Action OnEnter;

    public bool active { get; set; }
    private bool opened = false;

    [SerializeField] private new ParticleSystem particleSystem;

    private void Start () {
        StartCoroutine(AnimateMaterial(GetComponent<MeshRenderer>().materials[1]));
        GameManager.instance.GetPlayer().sequenceManager.player.OnSuccess += SequencePlayer_OnSuccess;
    }

    private IEnumerator AnimateMaterial (Material portalMaterial) {
        Color color = portalMaterial.color;
        color.a = 0f;
        portalMaterial.color = color;

        yield return new WaitUntil(() => active);

        while (true) {
            color = opened ? Color.yellow : Color.cyan;
            color.a = 0.4f * Mathf.Sin(3f * Time.time) + 0.5f;
            portalMaterial.color = color;
            yield return null;
        }
    }

    private void OnTriggerStay (Collider other) {
        if (other.tag.Equals("Player") && opened) {
            Enter();
        }
    }

    private void SequencePlayer_OnSuccess () {
        if(active && !opened)
            Open();
    }

    private void Open () {
        opened = true;
        particleSystem.Play();
        GameManager.instance.GetPlayer().sequenceManager.player.OnSuccess -= SequencePlayer_OnSuccess;
    }

    private void Enter () {
        if (OnEnter != null)
            OnEnter();

        particleSystem.Play();
        GameManager.instance.EndLevel();
    }
}
