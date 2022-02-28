using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Events
{
    public abstract class Event<T> : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private List<Listener<T>> eventListeners;

        void Awake() {
          if (eventListeners == null) {
            eventListeners = new List<Listener<T>>();
          }
        }

        public void Raise(T value)
        {
            for(int i = eventListeners.Count -1; i >= 0; i--)
                if (eventListeners[i] != null) {
                    eventListeners[i].OnEventRaised(value);
                } else {
                    UnregisterListener(eventListeners[i]);
                }
        }

        public void RegisterListener(Listener<T> listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(Listener<T> listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}