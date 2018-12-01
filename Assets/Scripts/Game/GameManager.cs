using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance { get; private set; }

    [SerializeField] private Player player = null;
    [SerializeField] private int maximumMonsterCount = 30;

    [SerializeField] private int[] maxEntityCount = new int[] {10,10,10};
    public int[] MaxEntityCount { get { return maxEntityCount;}}
    private int[] entityCount = new int[] { 0, 0, 0 };

    [SerializeField] private float survivalTime = 30f;
    [SerializeField] private SceneName nextLevel = SceneName.Level2;
    [SerializeField] private bool tutorial = false;

    public ExitPortal exitPortal { get; private set;}
    public bool levelEnded { get; private set; }

    private List<Spawner> spawners = new List<Spawner>();

    public float timer { get;  set; }
    public int killCount { get; private set; }

    private void Start () {
        instance = this;

        exitPortal = FindObjectOfType<ExitPortal>();
        exitPortal.active = false;
        spawners.AddRange(FindObjectsOfType<Spawner>());

        levelEnded = false;

        if(!tutorial)
            player.GetComponent<SequenceManager>().Invoke("StartGamePhase", 2f);

        timer = survivalTime;
    }

    private void Update () {

        if (!levelEnded && timer > 0) {
            timer -= Time.deltaTime;
            timer = Mathf.Max(0, timer);
        }

        if (timer == 0 && !exitPortal.active && !tutorial) {
            exitPortal.active = true;
        }
    }

    public Player GetPlayer () {
        return player;
    }

    public bool AllowMonsterCreation (int entityIndex) {
        return entityCount[entityIndex] < maxEntityCount[entityIndex];
    }

    public void AddEnemy (int entityIndex) {
        entityCount[entityIndex]++;
    }

    public void RemoveEnemy (int entityIndex) {
        killCount++;
        entityCount[entityIndex]--;
    }

    public void EndLevel () {
        levelEnded = true;
        // Make the player unreachable by enemies
        player.transform.position += Vector3.up * 10;
        player.gameObject.SetActive(false);
    }

    public void StartNextLevel() {
        SoundController.Instance.PlaySound(SoundName.UIButton4);
        SceneController.Instance.storedItems = ItemManager.Instance.RetrieveSelectedItems();
        SceneController.Instance.LoadScene(nextLevel);
    }
}
