using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    ArcadeKart playerKart;

    [SerializeField]
    List<Transform> levelCheckpoints;

    void Start()
    {
        if (!playerKart)
        {
            playerKart = FindObjectOfType<ArcadeKart>();
        }

        pointCount = levelCheckpoints.Count;

        float x1 = levelCheckpoints[0].transform.position.x;
        float z1 = levelCheckpoints[0].transform.position.z;
        float x2 = levelCheckpoints[1].transform.position.x;
        float z2 = levelCheckpoints[1].transform.position.z;

        for (int i = 0; i < pointCount; i++)
        {
            TrackLengh += LineLength(x1, z1, x2, z2);

            x1 = x2;
            z1 = z2;
            x2 = levelCheckpoints[(i + 1) % pointCount].transform.position.x;
            z2 = levelCheckpoints[(i + 1) % pointCount].transform.position.z;
        }
    }

    private int pointCount;
    private float TrackLengh = 0;
    private float TraveledLengh = 0;
    private float TotalLengh = 0;
    private int n = 0;

    void Update()
    {
        int Nn = (n + 1) % pointCount;

        float Xn = levelCheckpoints[n].transform.position.x;
        float Zn = levelCheckpoints[n].transform.position.z;

        float XNn = levelCheckpoints[Nn].transform.position.x;
        float ZNn = levelCheckpoints[Nn].transform.position.z;

        float PointLength = LineLength(Xn, Zn, XNn, ZNn);

        Transform p1 = levelCheckpoints[n];
        Transform p2 = levelCheckpoints[Nn];
        float Kp = Kcal(p1, p2);
        float Mp = Mcal(p1.transform.position.x, p1.transform.position.z, Kp);

        float Xb = playerKart.transform.position.x;
        float Zb = playerKart.transform.position.z;
        float Kb = -1/Kp;
        float Mb = Mcal(Xb, Zb, Kb);

        float X = (Mp - Mb) / (Kb - Kp);
        float Z = Kp * X + Mp;

        float procent = LineLength(X, Z, levelCheckpoints[n].transform.position.x, levelCheckpoints[n].transform.position.z) / TrackLengh;

        if (procent < 0)
        {
            n = (pointCount + n - 1) % pointCount;
            TraveledLengh -= PointLength;
        }

        if (procent >= 1)
        {
            n = (n + 1) % pointCount;
            TraveledLengh += PointLength;
        }

        TotalLengh = TraveledLengh / TrackLengh + procent;

        Debug.Log(LineLength(X, Z, levelCheckpoints[n].transform.position.x, levelCheckpoints[n].transform.position.z));
        Debug.Log(X);
        Debug.Log(Z);
        //Debug.Log(TrackLengh);
        //Debug.Log(procent + "%");
        //Debug.Log(TotalLengh);
        //Debug.Log("Point " + n);
    }

    private float LineLength(float oneX, float oneZ, float twoX, float twoZ)
    {
        float deltaX = (twoX - oneX);
        float deltaZ = (twoZ - oneZ);
        float Length = Mathf.Sqrt((deltaX * deltaX) + (deltaZ * deltaZ));

        return Length;
    }

    private float Kcal(Transform one, Transform two)
    {
        float deltaX = (two.transform.position.x - one.transform.position.x);
        float deltaZ = (two.transform.position.z - one.transform.position.z);
        float K = deltaX / deltaZ;

        return K;
    }

    private float Mcal(float X, float Z, float K)
    {
        float M = Z - K * X;

        return M;
    }
}
