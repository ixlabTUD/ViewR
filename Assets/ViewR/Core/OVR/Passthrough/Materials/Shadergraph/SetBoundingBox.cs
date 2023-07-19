using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetBoundingBox : MonoBehaviour
{

    private Bounds bounds;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        bounds = GetComponent<MeshRenderer>().bounds;
        GetComponent<MeshRenderer>().sharedMaterial.SetVector("minBox", bounds.min);
        GetComponent<MeshRenderer>().sharedMaterial.SetVector("maxBox", bounds.max);
        GetComponent<MeshRenderer>().sharedMaterial.SetVector("size", bounds.size);
    }
}
