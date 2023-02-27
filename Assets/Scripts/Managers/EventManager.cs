using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    private static readonly object eventManagerLock = new object();
    private static EventManager eventManager;
    private event Action<int> buttonPressed;
    private event Action<int> buttonUnpressed;

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
    public Action<int> ButtonPressed {
        get => buttonPressed;
        set {
            buttonPressed = value;
        }
    }
    public Action<int> ButtonUnpressed {
        get => buttonUnpressed;
        set {
            buttonUnpressed = value;
        }
    }

    private void Awake() {
        eventManager = GetEventManager;
    }
}
