using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Game.UI
{
    [Serializable]
    public class Menu
    {
        public List<MenuItem> items;
    }

    [Serializable]
    public class MenuItem
    {
        public Sprite icon;
        public Color color;
        public string text;
        public FunkySheep.Events.SimpleEvent onClickEvent;
        public UnityEvent onClickUnityEvent;
    }
}

