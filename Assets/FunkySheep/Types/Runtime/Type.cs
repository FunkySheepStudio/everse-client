using UnityEngine;
using System;
namespace FunkySheep.Types
{
    public abstract class Type<T> : ScriptableObject
    {
        public T value;
    }
}