using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Game.UI.CircleMenu
{
    [Serializable]
    public class Button
    {
        public Sprite icon;
        [ColorUsageAttribute(true, true)]
        public Color color;
        public string text;
    }

    public class Selector : MonoBehaviour, IPointerEnterHandler
    {
        public List<Button> buttons;
        public GameObject prefab;
        List<GameObject> items = new List<GameObject>();

        /*private void Start()
        {
            foreach (Button button in buttons)
            {
                GameObject buttonGo = GameObject.Instantiate(prefab, transform);
                buttonGo.GetComponent<UnityEngine.UI.Image>().material.color = button.color;
                buttonGo.GetComponent<UnityEngine.UI.Image>().sprite = button.icon;
                items.Add(buttonGo);
            }
        }*/

        public void OnPointerEnter(PointerEventData eventData)
        {
            GetComponent<Canvas>().enabled = true;
            //int index = items.IndexOf(eventData.pointerEnter.gameObject);
            //GetComponent<UnityEngine.UI.Text>().text = buttons[index].text;
        }
    }
}
