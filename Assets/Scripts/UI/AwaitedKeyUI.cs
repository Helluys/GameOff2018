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
            if (InputManager.Instance.simonInputs.Contains(value))
            {
                image.color = InputManager.Instance.simonInputsColor[value];
            }
        }
    }

    private Text text { get { return GetComponentInChildren<Text>(); } }
    private Image image { get { return GetComponentInChildren<Image>(); } }

    private void Update () {
        if (InputManager.Instance.IsKeyDown(awaitedInput) && destroyOnPress)
        {
            SoundController.Instance.PlaySound(SoundName.UIButton2);
            Destroy(gameObject);
        }
    }
}
