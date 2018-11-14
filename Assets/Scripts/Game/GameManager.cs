using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance { get; private set; }

    [SerializeField] private Player player = null;
    [SerializeField] private int maximumMonsterCount = 30;

    private int currentMonsterCount = 0;

    private void Start () {
        instance = this;
    }

    public Player GetPlayer () {
        return player;
    }

    public bool AllowMonsterCreation () {
        return currentMonsterCount < maximumMonsterCount;
    }

    public void AddMonster () {
        currentMonsterCount++;
    }

    public void RemoveMonster () {
        currentMonsterCount--;
    }
}
