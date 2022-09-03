using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Game.UI.CircleMenu
{
    public class MenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Game.UI.MenuItem menuItem;
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
            transform.GetChild(0).GetComponent<Image>().color = menuItem.color;
            transform.GetChild(0).GetComponent<Image>().sprite = menuItem.icon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GetComponent<Image>().color = new Color(125, 125, 125);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GetComponent<Image>().color = new Color(0, 0, 0);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (menuItem.onClickEvent)
                menuItem.onClickEvent.Raise();

            menuItem.onClickUnityEvent.Invoke();
        }

        private void OnDisable()
        {
            GetComponent<Image>().color = new Color(0, 0, 0);
        }
    }

}
