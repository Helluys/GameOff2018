using System.Collections.Generic;

using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] private List<GameObject> spawnableObjects = new List<GameObject>();
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private SpawnSequence spawnSequence;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private bool loop;
    [SerializeField] private float initialDelay;

    private int currentSequenceIndex;
    private float nextSpawnTime;
    private bool ended;

    private void Start () {
        if (spawnSequence.count == 0)
            ended = true;
        else
            nextSpawnTime = initialDelay + spawnSequence[0].delay;
    }

    private void Update () {
        if (!ended && Time.time > nextSpawnTime && GameManager.instance.AllowMonsterCreation()) {
            Instantiate(spawnableObjects[spawnSequence[currentSequenceIndex].entityIndex], spawnTransform.position, spawnTransform.rotation);

            if (spawnEffect != null)
                Instantiate(spawnEffect, spawnTransform.position, spawnTransform.rotation);

            currentSequenceIndex++;
            if (currentSequenceIndex >= spawnSequence.count) {
                if (loop)
                    currentSequenceIndex = 0;
                else
                    ended = true;
            }

            if (!ended)
                nextSpawnTime = Time.time + spawnSequence[currentSequenceIndex].delay;
        }
    }
}
