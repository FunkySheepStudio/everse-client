using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Zombies
{
    public class Shoot : MonoBehaviour
    {
        List<Collider> colliders;
        private void Awake()
        {
            colliders = transform.GetComponentsInChildren<Collider>().ToList();
            foreach (Collider collider in colliders)
            {
                if (collider.transform.GetComponent<Game.Zombies.Shoot>() == null)
                {
                    collider.gameObject.AddComponent<Game.Zombies.Shoot>();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Bullets"))
            {
                this.GetComponentInParent<Animator>().enabled = false;
                this.GetComponent<Collider>().attachedRigidbody.AddForce(gameObject.transform.forward * 1000);
            }
        }
    }
}
