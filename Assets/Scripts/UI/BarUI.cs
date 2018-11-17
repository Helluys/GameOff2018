using UnityEngine;

public abstract class BarUI : MonoBehaviour {

    [SerializeField] private RectTransform healthImageRect = null;
    private RectTransform rectTransform;

    public abstract float value { get; }
    public abstract float maxValue { get; }

    private void Start () {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update () {
        float ratio = value / maxValue;
        healthImageRect.sizeDelta = new Vector2((ratio - 1f) * rectTransform.rect.width, 0f);
    }
}
