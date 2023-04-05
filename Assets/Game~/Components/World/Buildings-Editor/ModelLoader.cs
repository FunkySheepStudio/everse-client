using System.Collections.Generic;
using UnityEngine;
using FunkySheep.SimpleJSON;

namespace Game.Buildings.Editor
{
    public class Building
    {
        public string id;
        public string modelFile;
        public string materialsFile;
        public int lod;

        public Building(string id, int lod)
        {
            this.id = id;
            this.lod = lod;
        }
    }

    public class ModelLoader : MonoBehaviour
    {
        public string templateModelUrl = "http://localhost:8080/buildings_models/";
        public string templateMaterialUrl = "http://localhost:8080/buildings_materials/";

        List<Building> buildings = new List<Building>();

        public void onBuildingModelsReception(JSONNode jsonModels)
        {
            FunkySheep.SimpleJSON.JSONArray models = jsonModels["data"].AsArray;
            
            for (int i = 0; i < models.Count; i++)
            {
                Building building = buildings.Find(item => item.id == models[i]["building_id"] && item.lod == models[i]["lod_level"]);
                if (building == null)
                {
                    building = new Building(models[i]["building_id"], models[i]["lod_level"]);
                    buildings.Add(building);
                }

                switch ((string)models[i]["type"])
                {
                    case "model":
                        DownLoadModel(models[i], building);
                        break;
                    case "material":
                        DownLoadMaterial(models[i], building);
                        break;
                    default:
                        break;
                }
            }
        }

        void DownLoadModel(JSONNode model, Building building)
        {
            string url = templateModelUrl + model["_id"];

            StartCoroutine(FunkySheep.Network.Downloader.Download(url, (fileID, file) =>
            {
                string ObjfileName = Application.persistentDataPath + "/" + fileID;
                building.modelFile = ObjfileName;
                LoadBuilding(building);
            }));
        }

        void DownLoadMaterial(JSONNode material, Building building)
        {
            string url = templateMaterialUrl + material["_id"];

            StartCoroutine(FunkySheep.Network.Downloader.Download(url, (fileID, file) =>
            {
                string MtlfileName = Application.persistentDataPath + "/" + fileID;
                building.materialsFile = MtlfileName;
                LoadBuilding(building);
            }));
        }

        void LoadBuilding(Building building)
        {
            if (building.modelFile != null && building.materialsFile != null)
            {
                GameObject buildingGO = new FunkySheep.Obj.OBJLoader().Load(building.modelFile, building.materialsFile);
                GameObject existingBuildingGo = transform.Find(building.id).gameObject;
                LODGroup lodgroup = existingBuildingGo.GetComponent<LODGroup>();
                LOD[] lod = lodgroup.GetLODs();
                lod[2].renderers = new Renderer[1];
                lod[2].renderers[0] = existingBuildingGo.GetComponent<Renderer>();

                Renderer[] buildingGORenderers = buildingGO.GetComponentsInChildren<Renderer>();

                foreach (Renderer renderer in buildingGORenderers)
                {
                    renderer.gameObject.AddComponent<MeshCollider>();
                }

                Renderer[] renderers = buildingGORenderers;
                lod[building.lod].renderers = renderers;
                lod[building.lod].screenRelativeTransitionHeight = (building.lod + 100) - building.lod * 50;

                lodgroup.SetLODs(lod);
                lodgroup.RecalculateBounds();

                buildingGO.transform.SetParent(existingBuildingGo.transform, false);
            }
        }
    }

}

