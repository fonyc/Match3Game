using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Events/String")]
public class StringArgument_Event : ScriptableObject
{
    private List<Action<string>> _listeners = new();

    public void AddListener(Action<string> listener) => _listeners.Add(listener);

    public void RemoveListener(Action<string> listener) => _listeners.Remove(listener);

    public void TriggerEvents(string arg) => _listeners.ForEach(listener => listener(arg));
}
