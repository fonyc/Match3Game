using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Events/NoArgument")]
public class NoArgument_Event : ScriptableObject
{
    private List<Action> _listeners = new();

    public void AddListener(Action listener) => _listeners.Add(listener);

    public void RemoveListener(Action listener) => _listeners.Remove(listener);

    public void TriggerEvents() => _listeners.ForEach(listener => listener());
}
