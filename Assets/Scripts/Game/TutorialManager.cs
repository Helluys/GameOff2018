using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {



    private void Start () {

    }

    private IEnumerator TutorialSequence () {
        yield return AwaitInput(new List<InputType>() {
            InputType.Up, InputType.Left, InputType.Right, InputType.Down
        });
    }

    private IEnumerator AwaitInput (List<InputType> expectedInput) {
        while (expectedInput.Count > 0) {
            List<InputType> foundInput = new List<InputType>();
            foreach (InputType input in expectedInput) {
                if (InputManager.Instance.IsKeyDown(input))
                    foundInput.Add(input);
            }

            expectedInput.RemoveAll(input => foundInput.Contains(input));

            if (expectedInput.Count > 0)
                yield return null;
        }

    }
}
