using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Singleton-Reference
    /// </summary>
    public static T Instance { get; protected set; }
    #endregion

    #region Methods
    /// <summary>
    /// Singleton-Setup
    /// </summary>
    public virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Singleton<" + typeof(T) + "> already exists! Destroying object " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instance = this as T;
    }

    /// <summary>
    /// Singleton-Destruction
    /// </summary>
    public virtual void OnDestroy()
    {
        if (Instance.Equals(this))
            Instance = null;
    }
    #endregion
}
