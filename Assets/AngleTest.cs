using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour
{

    public GameObject A;
    public GameObject B;

    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePosition = A.transform.position - B.transform.position;
        angle = Vector3.Angle(relativePosition, Vector3.up);
    }
}
