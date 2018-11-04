using System.Collections;
using UnityEngine;

public abstract class AIBehaviour : ScriptableObject {

    public abstract void OnStart (GameObject gameobject);
    public abstract IEnumerator Run ();

}
