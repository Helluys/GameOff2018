using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {

    [SerializeField] private Image healthImage = null;
    private Player player;
    private RectTransform rectTransform;

    private void Start () {
        this.player = GameManager.instance.GetPlayer();
        this.rectTransform = GetComponent<RectTransform>();
    }

    private void Update () {
        float ratio = this.player.instanceStatistics.health / this.player.sharedStatistics.maxHealth;
        this.healthImage.GetComponent<RectTransform>().sizeDelta = new Vector2((ratio - 1f) * this.rectTransform.rect.width, 0f);
    }
}
