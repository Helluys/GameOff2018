using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence {

    #region Variables
    public int sequencePossibility { get; private set; } 
    private List<int> list;
    private List<int> memory;
    #endregion

    #region Methods
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="possibility"></param>
    public Sequence(int possibility = 4)
    {
        sequencePossibility = possibility;
        Init();
    }
    /// <summary>
    /// Init a new sequence with a first value in it
    /// </summary>
    public void Init()
    {
        list = new List<int>();
        memory = new List<int>();
        StartNew();
    }
    /// <summary>
    /// Gets the current sequence as a list of int
    /// </summary>
    /// <returns></returns>
    public List<int> GetList()
    {
        return list;
    }
    /// <summary>
    /// Clear the current sequence and start a new one
    /// </summary>
    public void StartNew()
    {
        list.Clear();
        memory.Clear();
        list.Add(Random.Range(0, sequencePossibility));
    }
    /// <summary>
    /// Add new random values to the sequence
    /// </summary>
    /// <param name="amount">Amount of values to add to the sequence</param>
    public void Complexify(int amount = 1)
    {
        for(int i = 0; i < amount; i++)
        {
            // If nothing in memory add random
            if (memory.Count == 0)
            {
                list.Add(Random.Range(0, sequencePossibility));
                continue;
            }
            // Else add the one from the memory
            list.Add(memory[memory.Count-1]);
            memory.RemoveAt(memory.Count-1);
        }
    }
    /// <summary>
    /// Reduce the length of the sequence
    /// </summary>
    /// <param name="amount">Amount of values to remove from the sequence</param>
    public void UnComplexify(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            if (list.Count < 2)
                break;
            memory.Add(list[list.Count - 1]);
            list.RemoveAt(list.Count - 1);
        }
    }

    public void Repeat(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            memory.Add(list[list.Count - 1]);
        }
    }
    /// <summary>
    /// Check if the given value correspond to the one in the sequence at the given index
    /// </summary>
    /// <param name="index">Index of the value in th e sequence</param>
    /// <param name="value">Value to check</param>
    /// <returns>Whether the values correspond or not</returns>
    public bool Verify(int index,int value)
    {
        if (index < 0 || index > list.Count - 1)
            return false;
        return (list[index] == value);
    }
    #region Debug
    public void DebugDisplay()
    {
        string seq = "Sequence: ";
        for(int i = 0; i < list.Count; i++)
        {
            seq += list[i] + ", ";
        }
        Debug.Log(seq);
    }
    #endregion
    #endregion
}
