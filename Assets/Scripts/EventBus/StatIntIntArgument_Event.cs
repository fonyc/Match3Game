using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Events/StatIntInt")]
public class StatIntIntArgument_Event : ScriptableObject
{
    private List<Action<Stats, int, int>> _listeners = new();

    public void AddListener(Action<Stats, int, int> listener) => _listeners.Add(listener);

    public void RemoveListener(Action<Stats, int, int> listener) => _listeners.Remove(listener);

    public void TriggerEvents(Stats stats, int hits, int attackerColor) => _listeners.ForEach(listener => listener(stats, hits, attackerColor));
}
