using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewR.Core.Calibration.Aligner;

public class ChangeColorOnKabsch : MonoBehaviour
{

    public Color kabschColor;

    public Color twoPointColor;

    public MeshRenderer meshStation;
    // Start is called before the first frame update
    void Start()
    {
        Aligner.kabschPerformedEvent += changeColorToKabsch;
        Aligner.twoPointPerformedEvent += changeColorToTwoPoint;
        
    }
    
    private void OnDisable()
    {
        Aligner.kabschPerformedEvent -= changeColorToKabsch;
        Aligner.twoPointPerformedEvent -= changeColorToTwoPoint;
    }

    void changeColorToKabsch()
    {
        meshStation.material.SetColor("_Color", kabschColor);
    }

    void changeColorToTwoPoint()
    {
        meshStation.material.SetColor("_Color", twoPointColor);
    }
}
