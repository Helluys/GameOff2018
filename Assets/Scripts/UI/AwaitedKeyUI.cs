using UnityEngine;
using UnityEngine.UI;

public class AwaitedKeyUI : MonoBehaviour {

    public bool destroyOnPress { get; set; }

    private InputType _awaitedInput;
    public InputType awaitedInput {
        get { return _awaitedInput; }
        set {
            _awaitedInput = value;
            text.text = InputManager.Instance.GetKey(value).ToString();
        }
    }

    private Text text { get { return GetComponentInChildren<Text>(); } }

    private void Update () {
        if (InputManager.Instance.IsKeyDown(awaitedInput) && destroyOnPress)
            Destroy(gameObject);
    }
}
