using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ButtonTypePair
{
    public Button button;
    public InputType type;
}

public class InputManagerUI : MonoBehaviour {

    private bool IsWaitingForInput { get { return InputManager.Instance.isWaitingForInput; } }
    [SerializeField] private ButtonTypePair[] editorButtons;
    private Dictionary<InputType, Button> buttons;
    private bool updateColor = false;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        buttons = new Dictionary<InputType, Button>();
        foreach (ButtonTypePair button in editorButtons)
        {
            buttons.Add(button.type, button.button);
            UpdateButtonDisplay(button.type);
            button.button.onClick.AddListener(() => SetKey(button));
        }

        InputManager.Instance.KeySetEvent += UpdateButtonDisplay;
        updateColor = true;
    }

    private void OnDestroy()
    {
        if(InputManager.Instance != null)
        InputManager.Instance.KeySetEvent -= UpdateButtonDisplay;
    }


    private void SetKey(ButtonTypePair button)
    {
        if (IsWaitingForInput)
            return;

        InputManager.Instance.SetKey(button.type);
        button.button.GetComponentInChildren<Text>().text = "";
        Image image = button.button.GetComponent<Image>();
        DimImage(0.8f, ref image);
        Debug.Log("7");
        SoundController.Instance.PlaySound(SoundName.UIButton4);
       
    }

    private void UpdateButtonDisplay(InputType type)
    {
        Button button;
        if (buttons.TryGetValue(type, out button))
        {
            if (updateColor)
            {
                Image image = button.GetComponent<Image>();
                DimImage(1.25f, ref image);
            }
            button.GetComponentInChildren<Text>().text = InputManager.Instance.GetKey(type).ToString();
        }
    }

    private void DimImage(float factor,ref Image image)
    {
        Color color = image.color;
        color.r *= factor;
        color.g *= factor;
        color.b *= factor;
        image.color = color;
    }
}
