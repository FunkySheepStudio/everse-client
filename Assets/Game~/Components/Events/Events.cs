using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public class Events : MonoBehaviour
    {
        public FunkySheep.Logos.Manager logosManager;
        // Start is called before the first frame update
        void Start()
        {
            Add();
        }

        public void Add()
        {
            GameObject goLogo = logosManager.Add("cinema", logosManager.gameObject);
            goLogo.transform.localScale = new Vector3(3, 3, 3);
            goLogo.transform.position += Vector3.up * 15;
            goLogo.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        }
    }

}
