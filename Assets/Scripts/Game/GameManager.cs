using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance { get; private set; }

    [SerializeField] private Player player = null;
    [SerializeField] private int maximumMonsterCount = 30;
    [SerializeField] private float survivalTime = 30f;

    [SerializeField] private GameObject levelLoaderPrefab;
    [SerializeField] private string nextLevel = "SampleScene";

    private ExitPortal exitPortal;
    public bool levelEnded { get; private set; }

    private List<Spawner> spawners = new List<Spawner>();
    private List<Enemy> enemies = new List<Enemy>();

    public float timer { get; private set; }
    public int killCount { get; private set; }

    private void Start () {
        instance = this;

        exitPortal = FindObjectOfType<ExitPortal>();
        exitPortal.active = false;
        spawners.AddRange(FindObjectsOfType<Spawner>());

        levelEnded = false;
    }

    private void Update () {
        if (!levelEnded) {
            timer += Time.deltaTime;
        }

        if (timer > survivalTime && !exitPortal.active) {
            exitPortal.active = true;
        }
    }

    public Player GetPlayer () {
        return player;
    }

    public bool AllowMonsterCreation () {
        return enemies.Count < maximumMonsterCount;
    }

    public void AddEnemy (Enemy enemy) {
        enemies.Add(enemy);
    }

    public void RemoveEnemy (Enemy enemy) {
        killCount++;
        enemies.Remove(enemy);
    }

    internal void EndLevel () {
        levelEnded = true;
        // Make the player unreachable by enemies
        player.transform.position += Vector3.up * 10;
        player.gameObject.SetActive(false);
    }

    public void StartNextLevel() {
        ItemManager.Instance.ApplyItemChanges();
        Instantiate(levelLoaderPrefab).GetComponent<LevelLoader>().StartLoading(nextLevel);
    }
}
