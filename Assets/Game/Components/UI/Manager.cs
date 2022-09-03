using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    [AddComponentMenu("Game/UI/Manager")]
    public class Manager : MonoBehaviour
    {
        public Camera cam;
        public List<GameObject> loadedUis = new List<GameObject>();
        public GameObject circularMenu;

        public void Awake()
        {
            Game.Manager.Instance.UIManager = this;
        }

        public GameObject Load(GameObject ui)
        {
            GameObject instanciatedUI = loadedUis.Find(item => item == ui);
            if (!instanciatedUI)
            {
                instanciatedUI = GameObject.Instantiate(ui, transform);
                loadedUis.Add(instanciatedUI);
            } else
            {
                instanciatedUI.SetActive(true);
            }
            return instanciatedUI;
        }

        public void UnLoad(GameObject ui)
        {
            ui.SetActive(false);
        }
    }
}
