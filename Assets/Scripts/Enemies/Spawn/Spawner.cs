using System.Collections.Generic;

using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] private List<GameObject> spawnableObjects = new List<GameObject>();
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private SpawnSequence spawnSequence;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private bool loop;
    [SerializeField] private float initialDelay;

    private int currentSequenceIndex = 0;
    private float nextSpawnTime;
    private bool ended = false;

    private void Start () {
        if (spawnSequence.count == 0)
            ended = true;
        else
            nextSpawnTime = initialDelay + spawnSequence[0].delay;
    }

    private int entityIndex;

    private void Update () {

        if (!ended && Time.time > nextSpawnTime)
        {
            entityIndex = spawnSequence[currentSequenceIndex].entityIndex;
            if (GameManager.instance.AllowMonsterCreation(entityIndex))
            {
                if (entityIndex != 0)
                    GameManager.instance.Nani(entityIndex);

                GameObject go = Instantiate(spawnableObjects[entityIndex], spawnTransform.position, spawnTransform.rotation);
                GameManager.instance.AddEnemy(entityIndex);
                int index = entityIndex;
                go.GetComponent<Enemy>().OnDeath += (Enemy enemy) => GameManager.instance.RemoveEnemy(index);

                if (spawnEffect != null)
                    Instantiate(spawnEffect, spawnTransform.position, spawnTransform.rotation);
            }

            currentSequenceIndex++;
            if (currentSequenceIndex >= spawnSequence.count)
            {
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
