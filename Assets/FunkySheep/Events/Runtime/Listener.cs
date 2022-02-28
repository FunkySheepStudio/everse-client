using UnityEngine;
using UnityEngine.Events;

namespace FunkySheep.Events
{
  public abstract class Listener<T> : MonoBehaviour
  {
    [Tooltip("Event to register with.")]
    public Event<T> Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<T> Response;

    void Awake() {
      if (!Event) {
        Event = ScriptableObject.CreateInstance<Event<T>>();
      }

      if (Response == null) {
        Response = new UnityEvent<T>();
      }
    }

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T value)
    {
      Response.Invoke(value);
    }
  }
}