using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Events/TripleInt")]
public class TripleIntArgument_Event : ScriptableObject
{
    private List<Action<int, int, int>> _listeners = new();

    public void AddListener(Action<int, int, int> listener) => _listeners.Add(listener);

    public void RemoveListener(Action<int, int, int> listener) => _listeners.Remove(listener);

    public void TriggerEvents(int arg1, int arg2, int arg3) => _listeners.ForEach(listener => listener(arg1, arg2, arg3));
}
