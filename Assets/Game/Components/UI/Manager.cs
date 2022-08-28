using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using FunkySheep.Types;

namespace Game.UI
{
    [AddComponentMenu("Game/UI/Manager")]
    public class Manager : Singleton<Manager>
    {
        public UIDocument rootDocument;

        public override void Awake()
        {
            base.Awake();
            rootDocument = GetComponent<UIDocument>();
        }
    }
}
