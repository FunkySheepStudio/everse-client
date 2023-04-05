using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI.Aim
{
    public class Receive : MonoBehaviour
    {
        public UnityEvent invokeMethod;
        public UnityEvent invokeExitMethod;

        public void onAimReceived()
        {
            if (invokeMethod != null)
            {
                invokeMethod.Invoke();
            }
        }

        public void onAimExit()
        {
            if (invokeExitMethod != null)
            {
                invokeExitMethod.Invoke();
            }
        }
    }
}
