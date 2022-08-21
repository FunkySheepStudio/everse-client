using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Buildings.Editor
{
    public class ModelLoader : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            string fileName = Application.persistentDataPath + "/" + "New" + ".obj";
            new Dummiesman.OBJLoader().Load(fileName);
        }
    }

}
