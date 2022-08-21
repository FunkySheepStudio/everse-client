using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Buildings.Editor
{
    public class Manager : MonoBehaviour
    {
        private void Start()
        {
            Export(gameObject);
        }

        public void Export(GameObject building)
        {
            ObjExporter exporter = new ObjExporter();
            string filename = exporter.DoExport(building, false);
            StartCoroutine(Upload(filename));
        }

        IEnumerator Upload(string filename)
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            byte[] arr = File.ReadAllBytes(filename);
            formData.Add(new MultipartFormFileSection("Building_Model", arr));

            UnityWebRequest request = UnityWebRequest.Post("http://localhost:8080/buildings_models", formData);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"<color=red>Upload error: {request.error}</color>");
            }
            else
            {
                Debug.Log("<color=cyan>Upload successful</color>");
            }
        }
    }
}
