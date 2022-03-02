using UnityEngine;
using UnityEngine.Events;

namespace FunkySheep.Events
{
  [AddComponentMenu("FunkySheep/Listeners/Simple Listener")]
  public class SimpleListener : MonoBehaviour
  {
    [Tooltip("Event to register with.")]
    public SimpleEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent Response;

    void Awake() {
      if (!Event) {
        Event = ScriptableObject.CreateInstance<SimpleEvent>();
      }

      if (Response == null) {
        Response = new UnityEvent();
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

    public void OnEventRaised()
    {
      Response.Invoke();
    }
  }
}