using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Match3/Event")]
public class EventBus : ScriptableObject
{
    private List<Action> _listeners = new();

    public void AddListener(Action listener) => _listeners.Add(listener);

    public void RemoveListener(Action listener) => _listeners.Remove(listener);

    public void TriggerEvents() => _listeners.ForEach(listener => listener());
}
