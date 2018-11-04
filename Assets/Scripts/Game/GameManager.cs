using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private Player player = null;

    public static GameManager instance { get; private set; }

    private void Start () {
        instance = this;
    }

    public Player GetPlayer () {
        return player;
    }
}
