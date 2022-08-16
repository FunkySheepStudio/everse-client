using System.Collections.Generic;
using UnityEngine;


public class PowerManager : MonoBehaviour
{
    public int powerIndex;
    FunkySheep.Logos.Manager logoManager;

    private void Awake()
    {
        logoManager = GetComponent<FunkySheep.Logos.Manager>();
        powerIndex = Random.Range(1, 6);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject power = logoManager.Add(transform.parent.parent.parent.GetComponent<Game.PlaneRace.Manager>().powerNames[powerIndex], gameObject);
        power.GetComponent<MeshRenderer>().material.color = transform.parent.parent.parent.GetComponent<Game.PlaneRace.Manager>().powerColors[powerIndex];
    }

    private void OnTriggerEnter(Collider player)
    {
        transform.parent.parent.parent.GetComponent<Game.PlaneRace.Manager>().AddPower(player.gameObject, powerIndex);
    }
}

