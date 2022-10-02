using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Events/Int")]
public class IntArgument_Event : ScriptableObject
{
    private List<Action<int>> _listeners = new();

    public void AddListener(Action<int> listener) => _listeners.Add(listener);

    public void RemoveListener(Action<int> listener) => _listeners.Remove(listener);

    public void TriggerEvents(int arg) => _listeners.ForEach(listener => listener(arg));
}
