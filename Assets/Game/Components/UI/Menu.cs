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
        [SerializeField]
        Sprite icon;
        [SerializeField]
        string text;
        [SerializeField]
        UnityEvent callback;
    }
}

