using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Buildings.Editor
{
    public class ModelLoader : MonoBehaviour
    {
        public FunkySheep.Network.Services.Find buildingsModelFind;
        // Start is called before the first frame update
        void Start()
        {
            string ObjfileName = Application.persistentDataPath + "/" + "New" + ".obj";
            string MtlfileName = Application.persistentDataPath + "/" + "New" + ".mtl";
            new FunkySheep.Obj.OBJLoader().Load(ObjfileName, MtlfileName);
        }
    }

}
