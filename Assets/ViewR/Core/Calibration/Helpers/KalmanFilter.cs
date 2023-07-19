using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalmanFilter
{
    private bool isFirst = true;
    private bool haveSetFirst = false;
    private Vector3 xhat;
    private float P;
    private Vector3 z0;
    // private float Q = 1e-5f;
    // private float R = 0.0001f;
    //
    private float Q = 1e-11f;
    private float R = 0.00000001f;
    
    public Vector3[] Filter(Vector3[] z)
    {
        Vector3[] xhat = new Vector3[z.Length];
        xhat[0] = z[0];
        float P = 1;

        for (int k = 1; k < xhat.Length; k++)
        {
            Vector3 xhatminus = xhat[k - 1];
            float Pminus = P + Q;
            float K = Pminus / (Pminus + R);
            xhat[k] = xhatminus + K * (z[k] - xhatminus);
            P = (1 - K) * Pminus;

        }

        return xhat;
    }


    public void SetFirst(Vector3 z0)
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
        xhat = Vector3.zero;
        P = 0;
        z0 = Vector3.zero;
        Q = 1e-5f;
        R = 0.0001f;

    }
}
