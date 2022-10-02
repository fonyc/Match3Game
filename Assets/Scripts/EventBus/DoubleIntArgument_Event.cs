using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Events/DoubleInt")]
public class DoubleIntArgument_Event : ScriptableObject
{
    private List<Action<int, int>> _listeners = new();

    public void AddListener(Action<int, int> listener) => _listeners.Add(listener);

    public void RemoveListener(Action<int, int> listener) => _listeners.Remove(listener);

    public void TriggerEvents(int arg1, int arg2) => _listeners.ForEach(listener => listener(arg1, arg2));
}
