using UnityEngine;
using UnityEngine.UI;

public class CenterCast : MonoBehaviour
{
    public LayerMask myLayerMask;
    GameObject previousHit;
    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, -10f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, myLayerMask))
        {
            if (previousHit != hit.transform.gameObject)
            {
                if (previousHit)
                {
                    SendExit();
                }

                previousHit = hit.transform.gameObject;
                SendEnter();
            }
        } else
        {
            if (previousHit)
            {
                SendExit();
                previousHit = null;
            }
        }
    }

    void SendEnter()
    {
        Game.UI.Aim.Receive receivedComponent = previousHit.transform.gameObject.GetComponent<Game.UI.Aim.Receive>();
        if (receivedComponent != null)
        {
            GetComponent<Image>().color = Color.red;
            receivedComponent.onAimReceived();
        }
    }

    void SendExit()
    {
        Game.UI.Aim.Receive lastReceivedComponent = previousHit.transform.gameObject.GetComponent<Game.UI.Aim.Receive>();
        if (lastReceivedComponent != null)
        {
            GetComponent<Image>().color = Color.white;
            lastReceivedComponent.onAimExit();
        }
    }
}
