using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Хранит в себе события для их вызова
/// </summary>
public static class EventsBus
{
    private static readonly Dictionary<Type, List<Action<object>>> eventSubscriptions = new Dictionary<Type, List<Action<object>>>();

    public static void Subscribe<T>(Action<T> eventHandler)
    {
        Type eventType = typeof(T);

        if (!eventSubscriptions.ContainsKey(eventType))
        {
            eventSubscriptions[eventType] = new List<Action<object>>();
        }

        eventSubscriptions[eventType].Add(obj => eventHandler((T)obj));
    }

    public static void Unsubscribe<T>(Action<T> eventHandler)
    {
        Type eventType = typeof(T);

        if (eventSubscriptions.TryGetValue(eventType, out var handlers))
        {
            handlers.RemoveAll(obj => obj.Equals(eventHandler));
        }
        else
        {
            Debug.LogWarning($"Event {eventType.Name} is not subscribed to in the EventBus.");
        }
    }

    public static void Publish<T>(T eventData)
    {
        Type eventType = typeof(T);

        if (eventSubscriptions.TryGetValue(eventType, out var handlers))
        {
            foreach (var handler in handlers)
            {
                handler.Invoke(eventData);
            }
        }
        else
        {
            Debug.LogWarning($"Event {eventType.Name} is not subscribed to in the EventBus.");
        }
    }
}


#region EVENT_CLASSES
public class OnRemoveAllEnemies
{

}

public class OnEnemySpawn
{
    public string prefabName;
}

public class OnEnemyDeath
{
    public BotBehaviour enemy;
}

public class OnRefreshUIText
{
    public string className;
    public string text;
}
#endregion