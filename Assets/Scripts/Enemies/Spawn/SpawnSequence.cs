using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSequence", menuName = "Game data/Spawn Sequence")]
public class SpawnSequence : ScriptableObject {

    public int count { get { return sequence.Count; } }

    public SpawnDescriptor this[int i] {
        get { return sequence[i]; }
    }

    [Serializable]
    public struct SpawnDescriptor {
        public int entityIndex;
        public float delay;
    }

    [SerializeField] private List<SpawnDescriptor> sequence;

}
