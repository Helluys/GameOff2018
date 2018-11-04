using System.Collections;
using UnityEngine;

public abstract class EnemyBehaviour : ScriptableObject {

    public abstract void OnStart (Enemy enemy);
    public abstract IEnumerator Run ();

}
