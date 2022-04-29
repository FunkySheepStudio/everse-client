using FunkySheep.Earth.Buildings;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Buildings
{
    public class Editor : MonoBehaviour
    {
        FunkySheep.Earth.Buildings.Floor floor;
        List<GameObject> stages = new List<GameObject>();
        private void Awake()
        {
            floor = GetComponent<FunkySheep.Earth.Buildings.Floor>();
            floor.GetInnerPoints();
        }

        private void Update()
        {
            if (stages.Count > floor.building.stagesCount)
            {
                RemStage();
            }

            if (stages.Count < floor.building.stagesCount)
            {
                AddStage();
            }
        }

        public void AddStage()
        {
            GameObject stage = new GameObject("stage-" + stages.Count.ToString());
            stages.Add(stage);


            Vector3 stagePosition = transform.position;
            stagePosition.y = floor.building.hightPoint.Value;
            stagePosition += Vector3.up * stages.Count * 2.5f;

            stage.transform.position = stagePosition;
            stage.transform.parent = transform;
            Stage stageComponent = stage.AddComponent<Stage>();
            stageComponent.floor = floor;
            stageComponent.Create();
        }

        public void RemStage()
        {
            GameObject lastFloor = stages[stages.Count - 1];
            stages.Remove(lastFloor);
            Destroy(lastFloor);
        }
    }
}
