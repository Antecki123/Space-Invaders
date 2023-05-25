using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Scriptable Objects/Events")]
public class GameEvent : ScriptableObject
{
    [SerializeField, TextArea] private string description;
    HashSet<GameEventListener> listeners = new();

    public void Invoke()
    {
        foreach (var globalListener in listeners)
            globalListener.RaiseEvent();
    }

    public void Register(GameEventListener eventListener) => listeners.Add(eventListener);
    public void Unregister(GameEventListener eventListener) => listeners.Remove(eventListener);
}