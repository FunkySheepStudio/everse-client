using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.UI
{
    [AddComponentMenu("Game/UI/Manager")]
    public class Manager : MonoBehaviour
    {
        public Camera cam;
        public Camera mainCam;
        public List<GameObject> loadedUis = new List<GameObject>();
        public GameObject circularMenu;

        public void Awake()
        {
            Game.Manager.Instance.UIManager = this;

            UniversalAdditionalCameraData cameraUIData = cam.GetUniversalAdditionalCameraData();
            cameraUIData.renderType = CameraRenderType.Overlay;

            UniversalAdditionalCameraData cameraData = mainCam.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(Game.Manager.Instance.UIManager.cam);
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
