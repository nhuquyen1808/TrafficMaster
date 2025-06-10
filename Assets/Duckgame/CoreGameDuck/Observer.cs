using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public enum EventAction
    {
        EVENT_CAR_DONE_ACTION,
        EVENT_GET_COIN_CAR,
        EVENT_CAR_TRIGGER,
        EVENT_CAR_DISABLE,
        EVENT_CAR_HIT_REDLIGHT,
        EVENT_UPDATE_COIN,
        EVENT_BUY_LANDMAT,
        EVENT_BUY_CARMAT,
    }

    public class Observer
    {
        public static Dictionary<string, List<Action<object>>> Listeners = new Dictionary<string, List<Action<object>>> { };

        public static void AddObserver(EventAction act, Action<object> callback)
        {
            if (!Listeners.ContainsKey(act.ToString()))
            {
                Listeners.Add(act.ToString(), new List<Action<object>>());
            }

            Listeners[act.ToString()].Add(callback);
        }
        public static void RemoveObserver(EventAction act, Action<object> callback)
        {
            if (!Listeners.ContainsKey(act.ToString()))
                return;
            Listeners[act.ToString()].Remove(callback);
        }
        public static void Notify(EventAction act, object datas)
        {
            if (!Listeners.ContainsKey(act.ToString()))
                return;

            foreach (var listener in Listeners[act.ToString()])
            {
                try
                {
                    listener?.Invoke(datas);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error on invoke " + e);
                }
            }
        }
    }
}