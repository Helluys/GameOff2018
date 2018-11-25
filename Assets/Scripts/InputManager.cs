using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    None = -1,

    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,

    Green = 4,
    Yellow = 5,
    Red = 6,
    Blue = 7,

    Item1 = 8,
    Item2 = 9,

    Roll = 10
}

[System.Serializable]
public struct TypeKeyCodePair
{
    public InputType type;
    public KeyCode key;
}

public class InputManager : SingletonBehaviour<InputManager> {

    [SerializeField] private TypeKeyCodePair[] editorKeys;

    public InputType[] simonInputs { get { return new InputType[] { InputType.Green, InputType.Yellow, InputType.Red, InputType.Blue }; } }
    public Dictionary<InputType, int> simonInputsValues = new Dictionary<InputType, int>()
    {
        { InputType.Green,0 },
        { InputType.Yellow,2 },
        { InputType.Red,1 },
        { InputType.Blue,3 },
    };

    private KeyCode pressedKey = KeyCode.None;
    public bool isWaitingForInput { get; private set; }
    private Event currentEvent { get { return Event.current; } }

    private Coroutine cSetKey;
    private Dictionary<InputType, KeyCode> Keys;

    public delegate void del_KeySetEvent(InputType type);
    public del_KeySetEvent KeySetEvent;


    public override void Awake()
    {
        base.Awake();
        isWaitingForInput = false;
        Keys = new Dictionary<InputType, KeyCode>();
        foreach (TypeKeyCodePair key in editorKeys)
        {
            Keys.Add(key.type, key.key);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public KeyCode GetKey(InputType type)
    {
        KeyCode key;
        if (Keys.TryGetValue(type, out key))
            return key;
        
        Debug.LogError("No key defined for this Input: " + type);
        return KeyCode.None;
    }

    public bool IsKeyDown(InputType type)
    {
        return Input.GetKeyDown(GetKey(type));
    }


    public bool IsKey(InputType type)
    {
        return Input.GetKey(GetKey(type));
    }

    private void SetKey(InputType type,KeyCode key)
    {
        if (!Keys.ContainsKey(type))
            Keys.Add(type,key);

        Keys[type] = key;
    }

    private void OnGUI()
    {
        if (!isWaitingForInput)
            return;

        if (currentEvent.type == EventType.KeyDown)
        {
            if (currentEvent.keyCode != KeyCode.None)
                pressedKey = currentEvent.keyCode;
        }
    }

    public void SetKey(InputType type)
    {
        if (isWaitingForInput)
            return;

        if (cSetKey != null)
            StopCoroutine(cSetKey);

        cSetKey = StartCoroutine(ESetKey(type));
    }

    private IEnumerator ESetKey(InputType type)
    {
        isWaitingForInput = true;
        yield return new WaitUntil(() => pressedKey != KeyCode.None);
        Keys[type] = pressedKey;
        pressedKey = KeyCode.None;
        isWaitingForInput = false;
        if(KeySetEvent != null)
        KeySetEvent(type);
    }
}
