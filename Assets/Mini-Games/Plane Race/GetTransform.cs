using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTransform : MonoBehaviour
{
    public FunkySheep.Types.Vector3 position;
    public FunkySheep.Types.Quaternion rotation;
    
    // Update is called once per frame
    void Update()
    {
        position.value = transform.position;
        rotation.value = transform.rotation;
    }
}
