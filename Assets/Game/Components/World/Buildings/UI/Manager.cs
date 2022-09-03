using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Buildings.UI
{
    public class Manager : MonoBehaviour
    {
        public Game.UI.Menu menu;

        public void LoadMenu()
        {
            Game.Manager.Instance.UIManager.circularMenu.GetComponent<Game.UI.CircleMenu.Manager>().Load(menu);
        }

        public void UnLoadMenu()
        {
            Game.Manager.Instance.UIManager.circularMenu.GetComponent<Game.UI.CircleMenu.Manager>().UnLoad();
        }
    }
}
