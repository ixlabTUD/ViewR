using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalmanFilterFloat
{
    private bool isFirst = true;
    private bool haveSetFirst = false;
    private float xhat;
    private float P;
     private float z0;
    // private float Q = 1e-7f;
    // private float R = 0.000001f;
    //
    private float Q = 1e-11f;
    private float R = 0.000000001f;
    
    public float[] Filter(float[] z)
    {
        float[] xhat = new float[z.Length];
        xhat[0] = z[0];
        float P = 1;

        for (int k = 1; k < xhat.Length; k++)
        {
            float xhatminus = xhat[k - 1];
            float Pminus = P + Q;
            float K = Pminus / (Pminus + R);
            xhat[k] = xhatminus + K * (z[k] - xhatminus);
            P = (1 - K) * Pminus;

        }

        return xhat;
    }


    public void SetFirst(float z0)
    {
        this.z0 = z0;
        haveSetFirst = true;
    }

    public void SetQ(float Q)
    {
        this.Q = Q;
    }

    public void SetR(float R)
    {
        this.R = R;
    }

    public void Reset()
    {
        isFirst = true;
        haveSetFirst = false;
        xhat = 0f;
        P = 0;
        z0 = 0f;
        Q = 1e-5f;
        R = 0.0001f;

    }
}
