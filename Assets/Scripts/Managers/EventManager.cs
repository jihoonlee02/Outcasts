using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    private static readonly object eventManagerLock = new object();
    private static EventManager eventManager;
    private event Action<int> activated;
    private event Action<int> deactivated;

    public static EventManager GetEventManager {
        get {
            if (eventManager == null) {
                lock (eventManagerLock) {
                    if (eventManager == null) {
                        eventManager = new EventManager();
                    }
                }
            }
            return eventManager;
        }
    }
    public Action<int> Activated {
        get => activated;
        set {
            activated = value;
        }
    }
    public Action<int> Deactivated {
        get => deactivated;
        set {
            deactivated = value;
        }
    }

    private void Awake() {
        eventManager = GetEventManager;
    }
}
