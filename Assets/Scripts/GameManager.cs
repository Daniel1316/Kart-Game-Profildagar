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

        PointK = new float[pointCount];
        PointM = new float[pointCount];
        LineK = new float[pointCount];
        //Cal TrackLength
        //Point R�ta linje
        for (int i = 0; i < pointCount; i++)
        {
            int Pi = (pointCount + i - 1) % pointCount;
            int Ni = (i + 1) % pointCount;

            TrackLength += LineLength(levelCheckpoints[i].position.x, levelCheckpoints[i].position.z, levelCheckpoints[Ni].position.x, levelCheckpoints[Ni].position.z);


            PointK[i] = -1/Kcal(levelCheckpoints[Pi].transform, levelCheckpoints[Ni].transform);
            PointM[i] = Mcal(levelCheckpoints[i].position.x, levelCheckpoints[i].position.z, PointK[i]);
            LineK[i] = Kcal(levelCheckpoints[i].transform, levelCheckpoints[Ni].transform);
        }
    }

    private int pointCount;
    private float TrackLength = 0;
    private float[] LineK;
    private float[] PointK;
    private float[] PointM;

    private int n = 0;
    public static float procent = 0;
    private float ClearedProcent = 0;

    private float Cut1X;
    private float Cut1Z;
    private float Cut2X;
    private float Cut2Z;

    void Update()
    {
        int Pn = (pointCount + n - 1) % pointCount;
        int Nn = (n + 1) % pointCount;

        Transform CPn = levelCheckpoints[Pn];
        Transform Cn = levelCheckpoints[n];
        Transform CNn = levelCheckpoints[Nn];

        Transform KartT = playerKart.transform;

        float KartK = LineK[n];
        float KartM = Mcal(KartT.position.x, KartT.position.z, KartK);

        Cut1X = CutX(KartK, KartM, PointK[n], PointM[n]);
        Cut1Z = CutZ(KartK, KartM, Cut1X);

        Cut2X = CutX(KartK, KartM, PointK[Nn], PointM[Nn]);
        Cut2Z = CutZ(KartK, KartM, Cut2X);

        float CutLength = LineLength(Cut1X, Cut1Z, Cut2X, Cut2Z);

        float TraveledProcent = LineLength(KartT.position.x, KartT.position.z, Cut1X, Cut1Z) / CutLength;

        float segmentLength = LineLength(Cn.position.x, Cn.position.z, CNn.position.x, CNn.position.z);
        float PasedsegmentLength = LineLength(CPn.position.x, CPn.position.z, Cn.position.x, Cn.position.z);

        if (TraveledProcent < 0f)
        {
            n = Pn;
            ClearedProcent -= PasedsegmentLength;
        }

        if (TraveledProcent >= 1f)
        {
            n = Nn;
            ClearedProcent += segmentLength;
        }

        procent = (ClearedProcent + segmentLength * TraveledProcent) / TrackLength;

        //Debug.Log(Cut1X + " Cut1X");
        //Debug.Log(Cut1Z + " Cut1Z");
        //Debug.Log(TraveledProcent + "%");
        //Debug.Log(ClearedProcent + "%");
        //Debug.Log(LineLength(Cn.position.x, Cn.position.z, CNn.position.x, CNn.position.z) + " Long");
        //Debug.Log(CutLength + " Long");
        //Debug.Log(LineLength(KartT.position.x, KartT.position.z, Cut1X, Cut1Z));
        Debug.Log("Point " + n);
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
        float deltaX = (two.position.x - one.position.x);
        float deltaZ = (two.position.z - one.position.z);
        float K = deltaZ / deltaX;

        return K;
    }

    private float Mcal(float X, float Z, float K)
    {
        float M = Z - K * X;

        return M;
    }

    private float CutX(float K1, float M1, float K2, float M2)
    {
        float X = (M1 - M2) / (K2 - K1);

        return X;
    }

    private float CutZ(float K, float M, float X)
    {
        float Z = K * X + M;
        return Z;
    }
}
