using UnityEngine;

namespace Game.Player
{
    public class Selector : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    if (hitInfo.transform.gameObject.tag == "Floor")
                    {
                        if (!hitInfo.transform.gameObject.GetComponent<Game.Buildings.Editor>())
                        {
                            hitInfo.transform.gameObject.AddComponent<Game.Buildings.Editor>();
                        }
                    }
                }
            }
        }
    }
}
