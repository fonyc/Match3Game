using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Events/Bool")]
public class BoolArgument_Event : ScriptableObject
{
    private List<Action<bool>> _listeners = new();

    public void AddListener(Action<bool> listener) => _listeners.Add(listener);

    public void RemoveListener(Action<bool> listener) => _listeners.Remove(listener);

    public void TriggerEvents(bool arg) => _listeners.ForEach(listener => listener(arg));
}
