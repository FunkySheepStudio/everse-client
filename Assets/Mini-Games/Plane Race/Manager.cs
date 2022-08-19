using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace Game.PlaneRace
{
    public enum Status
    {
        Creation,
        Testing,
        Playing,
        Over
    }

    public class PlayerPowers
    {
        public int power1 = 0;
        public int power2 = 0;
        public int power3 = 0;
    }

    public class Manager : MonoBehaviour
    {
        public FunkySheep.Types.Vector3 playerPosition;
        public FunkySheep.Types.Quaternion playerRotation;
        public GameObject gate;
        public List<GameObject> gates = new List<GameObject>();
        public int laps = 4;
        public List<VectorImage> powersImages;

        public VisualTreeAsset scoreUI;
        public VisualTreeAsset iconsUI;

        TemplateContainer scoreUIContainer;
        TemplateContainer iconUIContainer;

        public Dictionary<int, string> powerNames = new Dictionary<int, string>()
        {
            { 0, "none"},
            { 1, "slow" },
            { 2, "shield" },
            { 3, "next"},
            { 4, "mine" },
            { 5, "missile" },
            { 6, "speed"}
        };

        public Dictionary<int, Color> powerColors = new Dictionary<int, Color>()
        {
            { 0, Color.green},
            { 1, Color.yellow },
            { 2, Color.blue },
            { 3, Color.cyan},
            { 4, Color.red },
            { 5, Color.black },
            { 6, Color.green}
        };

        Dictionary<ulong, int> playersGatesCount = new Dictionary<ulong, int>();
        Dictionary<ulong, int> playerLaps = new Dictionary<ulong, int>();
        Dictionary<ulong, PlayerPowers> playersPowers = new Dictionary<ulong, PlayerPowers>();

        Status status = Status.Creation;

        Vector3 _lastPosition;

        private void Awake()
        {
            iconUIContainer = iconsUI.Instantiate();
            scoreUIContainer = scoreUI.Instantiate();

            Game.UI.Manager.Instance.rootDocument.rootVisualElement.Q<VisualElement>("Top").Add(scoreUIContainer);
            Game.UI.Manager.Instance.rootDocument.rootVisualElement.Q<VisualElement>("CenterRight").Add(iconUIContainer);
        }


        // Start is called before the first frame update
        void Start()
        {
            _lastPosition = playerPosition.value;
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(_lastPosition, playerPosition.value) >= 500 && status == Status.Creation)
            {
                SpawnGate();
                _lastPosition = playerPosition.value;
            }
        }

        void SpawnGate()
        {
            GameObject gateGo = GameObject.Instantiate(gate, transform);
            gateGo.transform.position = playerPosition.value;
            gateGo.transform.rotation = playerRotation.value;
            gates.Add(gateGo);
            gateGo.GetComponent<GateManager>().id = gates.Count - 1;
        }

        public void passGate(GameObject gate, GameObject player)
        {
            GateManager gateManager = gate.GetComponent<GateManager>();
            ulong playerId = player.GetComponent<Unity.Netcode.NetworkObject>().NetworkObjectId;

            if (gate == gates[0] && status == Status.Creation && gates.Count > 1)
            {
                status = Status.Testing;
            } else if (gate == gates[0] && status == Status.Creation)
            {
                for (int i = 0; i < 12; i++)
                {
                    gate.GetComponent<GateManager>().gateModel.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.yellow * 10;
                }
            }
            
            if (status == Status.Testing)
            {
                // Set the default parameters
                if (!playersGatesCount.ContainsKey(playerId))
                {
                    playersGatesCount.Add(playerId, 0);
                    playerLaps.Add(playerId, 1);
                }


                if (!gateManager.count.ContainsKey(playerId))
                {
                    gateManager.count.Add(playerId, 0);
                }

                if (!playersGatesCount.ContainsKey(playerId))
                {
                    playersGatesCount.Add(playerId, 0);
                }

                if (!playerLaps.ContainsKey(playerId))
                {
                    playerLaps.Add(playerId, 1);
                }

                // Add a gate if the current laps
                if (gateManager.count[playerId] < playerLaps[playerId])
                {
                    gateManager.count[playerId] += 1;
                    playersGatesCount[playerId] += 1;

                    gateManager.textComponent.text = gateManager.count[playerId].ToString();
                }

                for (int i = (playerLaps[playerId] - 1) * 12 / laps; i < 12 / laps * (playerLaps[playerId]) ; i++)
                {
                    gate.GetComponent<GateManager>().gateModel.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.red * 10;
                }

                // If all the gates have been passed
                if (playersGatesCount[playerId] == gates.Count)
                {
                    playersGatesCount[playerId] = 0;
                    if (playerLaps[playerId] != laps)
                    {
                        playerLaps[playerId] += 1;
                    } else
                    {
                        foreach (GameObject gateGo in gates)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                gateGo.GetComponent<GateManager>().gateModel.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.green * 10;
                            }
                        }

                        status = Status.Over;
                        //SceneManager.UnloadSceneAsync("Scenes/Wip/Mini games/Plane Race");
                    }
                }

                scoreUIContainer.Q<Label>("Gates").text = playersGatesCount[playerId] + " / " + gates.Count;
                scoreUIContainer.Q<Label>("Laps").text = playerLaps[playerId] + " / " + laps;
            }
        }

        public void AddPower(GameObject player, int powerIndex)
        {
            ulong playerId = player.GetComponent<Unity.Netcode.NetworkObject>().NetworkObjectId;
            if (!playersPowers.ContainsKey(playerId))
            {
                PlayerPowers playerPowers = new PlayerPowers();
                playersPowers.Add(playerId, playerPowers);
            }

            if (playersPowers[playerId].power1 == 0)
            {
                playersPowers[playerId].power1 = powerIndex;
                iconUIContainer.Q<Button>("Power1").style.backgroundImage = new StyleBackground(powersImages[powerIndex]);
            }
            else if (playersPowers[playerId].power2 == 0)
            {
                playersPowers[playerId].power2 = powerIndex;
                iconUIContainer.Q<Button>("Power2").style.backgroundImage = new StyleBackground(powersImages[powerIndex]);
            }
            else if (playersPowers[playerId].power3 == 0)
            {
                playersPowers[playerId].power3 = powerIndex;
                iconUIContainer.Q<Button>("Power3").style.backgroundImage = new StyleBackground(powersImages[powerIndex]);
            } else
            {
                playersPowers[playerId].power3 = playersPowers[playerId].power2;
                playersPowers[playerId].power2 = playersPowers[playerId].power1;
                playersPowers[playerId].power1 = powerIndex;

                iconUIContainer.Q<Button>("Power3").style.backgroundImage = iconUIContainer.Q<Button>("Power2").style.backgroundImage;
                iconUIContainer.Q<Button>("Power2").style.backgroundImage = iconUIContainer.Q<Button>("Power1").style.backgroundImage;
                iconUIContainer.Q<Button>("Power1").style.backgroundImage = new StyleBackground(powersImages[powerIndex]);
            }
        }
    }
}

