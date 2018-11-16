using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance { get; private set; }

    [SerializeField] private Player player = null;
    [SerializeField] private int maximumMonsterCount = 30;
    [SerializeField] private float survivalTime = 30f;

    private GameObject exitPortal;
    public bool levelEnded { get; private set; }

    private List<Spawner> spawners = new List<Spawner>();
    private List<Enemy> enemies = new List<Enemy>();

    public float timer { get; private set; }

    private void Start () {
        instance = this;

        exitPortal = FindObjectOfType<ExitPortal>().gameObject;
        exitPortal.SetActive(false);
        spawners.AddRange(FindObjectsOfType<Spawner>());

        levelEnded = false;
    }

    private void Update () {
        if (!levelEnded) {
            timer += Time.deltaTime;
        }

        if (timer > survivalTime && !exitPortal.activeSelf) {
            exitPortal.SetActive(true);
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
        enemies.Remove(enemy);
    }

    internal void EndLevel () {
        levelEnded = true;
        player.gameObject.SetActive(false);
        Debug.Log("Level finished!");
    }
}
