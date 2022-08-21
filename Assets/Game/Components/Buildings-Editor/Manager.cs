using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using FunkySheep.SimpleJSON;

namespace Game.Buildings.Editor
{
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Network.Services.Find findService;
        public FunkySheep.Network.Services.Create createService;

        public FunkySheep.Types.String osm_ID;
        public FunkySheep.Types.Double latitude;
        public FunkySheep.Types.Double longitude;

        private void Start()
        {
            Find();
        }

        public void Find()
        {
            JSONNode query = JSONNode.Parse("{ }");
            query["osm_id"] = gameObject.name;

            findService.query = query;
            findService.Execute();
        }

        public void OnFindResult(JSONNode result)
        {
            if (result["data"]["total"] == 0)
            {
                Create();
            }
        }

        public void Create()
        {
            osm_ID.value = gameObject.name;

            createService.Execute();
        }

        public void OnCreateResult(JSONNode result)
        {
            Export(gameObject);
        }

        public void Export(GameObject building)
        {
            ObjExporter exporter = new ObjExporter();
            string filename = exporter.DoExport(building, false);
            StartCoroutine(Upload(filename, building));
        }

        IEnumerator Upload(string filename, GameObject building)
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            byte[] arr = File.ReadAllBytes(filename);
            formData.Add(new MultipartFormFileSection(building.name, arr));

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
