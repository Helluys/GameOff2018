using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {

    [SerializeField] private Image healthImage;
    private Player player;
    private RectTransform rectTransform;

    // Use this for initialization
    private void Start () {
        this.player = GameManager.instance.GetPlayer();
        this.rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update () {
        float ratio = this.player.Health / this.player.MaxHealth;
        this.healthImage.GetComponent<RectTransform>().sizeDelta = new Vector2((ratio - 1f) * this.rectTransform.rect.width, 0f);
    }
}
