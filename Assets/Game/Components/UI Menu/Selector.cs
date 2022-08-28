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
        public void OnPointerEnter(PointerEventData eventData)
        {
            //eventData.hovered[0].GetComponent<UnityEngine.UI.Image>().material.SetColor("_Color", Color.black);
        }
    }
}
