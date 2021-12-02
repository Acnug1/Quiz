using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]

public class MyGridObjectEvent : UnityEvent<GridObject> { }

public class EventManager : MonoBehaviour
{
    private const string ErrorMessage = "There needs to be one active EventManger script on a GameObject in your scene.";
    private static EventManager _eventManager;
    private Dictionary<string, MyGridObjectEvent> _eventDictionary;

    public static EventManager Instance
    {
        get
        {
            if (!_eventManager)
            {
                _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!_eventManager)
                {
                    Debug.LogError(ErrorMessage);
                }
                else
                {
                    _eventManager.Init();
                }
            }

            return _eventManager;
        }
    }

    private void Init()
    {
        if (_eventDictionary == null)
        {
            _eventDictionary = new Dictionary<string, MyGridObjectEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction<GridObject> listener)
    {
        MyGridObjectEvent thisEvent = null;

        if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new MyGridObjectEvent();
            thisEvent.AddListener(listener);
            Instance._eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<GridObject> listener)
    {
        if (_eventManager == null) return;

        MyGridObjectEvent thisEvent = null;

        if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, GridObject arg)
    {
        MyGridObjectEvent thisEvent = null;

        if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(arg);
        }
    }
}