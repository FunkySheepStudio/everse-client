using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Events
{
    [CreateAssetMenu(menuName = "FunkySheep/Events/Simple Event")]
    public abstract class SimpleEvent : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private List<SimpleListener> eventListeners;

        void Awake() {
          if (eventListeners == null) {
            eventListeners = new List<SimpleListener>();
          }
        }

        public void Raise()
        {
            for(int i = eventListeners.Count -1; i >= 0; i--)
                if (eventListeners[i] != null) {
                    eventListeners[i].OnEventRaised();
                } else {
                    UnregisterListener(eventListeners[i]);
                }
        }

        public void RegisterListener(SimpleListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(SimpleListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}