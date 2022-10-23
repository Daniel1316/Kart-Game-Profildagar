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
        //Point Räta linje
        for (int i = 0; i < pointCount; i++)
        {
            int Pi = (pointCount + i - 1) % pointCount;
            int Ni = (i + 1) % pointCount;

            TrackLength += SegmentLength(levelCheckpoints[i].position.x, levelCheckpoints[i].position.z, levelCheckpoints[Ni].position.x, levelCheckpoints[Ni].position.z);


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
    private float ClearedLength = 0;
    public static float procent = 0;

    float TraveledProcent;
    float TraveledBackwordsProcent;
    float LineLength;
    float PasedLineLength;

    void Update()
    {
        //int Pn = (pointCount + n - 1) % pointCount;
        //int Nn = (n + 1) % pointCount;

        //Transform CPn = levelCheckpoints[Pn];
        //Transform Cn = levelCheckpoints[n];
        //Transform CNn = levelCheckpoints[Nn];

        //Transform KartT = playerKart.transform;

        //float KartK = LineK[n];
        //float KartM = Mcal(KartT.position.x, KartT.position.z, KartK);

        ////Cut Points Cords
        //float Cut1X = CutX(KartK, KartM, PointK[n], PointM[n]);
        //float Cut1Z = CutZ(KartK, KartM, Cut1X);

        //float Cut2X = CutX(KartK, KartM, PointK[Nn], PointM[Nn]);
        //float Cut2Z = CutZ(KartK, KartM, Cut2X);

        //float CutLength = SegmentLength(Cut1X, Cut1Z, Cut2X, Cut2Z);

        ////Stuf
        //TraveledProcent = SegmentLength(KartT.position.x, KartT.position.z, Cut1X, Cut1Z) / CutLength;
        //TraveledBackwordsProcent = SegmentLength(KartT.position.x, KartT.position.z, Cut2X, Cut2Z) / CutLength;

        //LineLength = SegmentLength(Cn.position.x, Cn.position.z, CNn.position.x, CNn.position.z);
        //PasedLineLength = SegmentLength(CPn.position.x, CPn.position.z, Cn.position.x, Cn.position.z);

        AllCalculations();

            if (TraveledProcent >= 1f)
            {
                n = (n + 1) % pointCount;
                ClearedLength += LineLength;
                AllCalculations();
            }
            else if (TraveledBackwordsProcent > 1f)
            {
                n = (pointCount + n - 1) % pointCount;
                ClearedLength -= PasedLineLength;
                AllCalculations();
            }


        procent = (ClearedLength + LineLength * TraveledProcent) / TrackLength;

        //Debug.Log(Cut1X + " Cut1X");
        //Debug.Log(Cut1Z + " Cut1Z");
        Debug.Log(TraveledProcent + "%");
        //Debug.Log(ClearedProcent + "%");
        //Debug.Log(LineLength(Cn.position.x, Cn.position.z, CNn.position.x, CNn.position.z) + " Long");
        //Debug.Log(CutLength + " Long");
        //Debug.Log(LineLength(KartT.position.x, KartT.position.z, Cut1X, Cut1Z));
        Debug.Log(procent);
        Debug.Log("Point " + n);
    }

    private void AllCalculations()
    {
        int Pn = (pointCount + n - 1) % pointCount;
        int Nn = (n + 1) % pointCount;

        Transform CPn = levelCheckpoints[Pn];
        Transform Cn = levelCheckpoints[n];
        Transform CNn = levelCheckpoints[Nn];

        Transform KartT = playerKart.transform;

        float KartK = LineK[n];
        float KartM = Mcal(KartT.position.x, KartT.position.z, KartK);

        //Cut Points Cords
        float Cut1X = CutX(KartK, KartM, PointK[n], PointM[n]);
        float Cut1Z = CutZ(KartK, KartM, Cut1X);

        float Cut2X = CutX(KartK, KartM, PointK[Nn], PointM[Nn]);
        float Cut2Z = CutZ(KartK, KartM, Cut2X);

        float CutLength = SegmentLength(Cut1X, Cut1Z, Cut2X, Cut2Z);

        //Stuf
        TraveledProcent = SegmentLength(KartT.position.x, KartT.position.z, Cut1X, Cut1Z) / CutLength;
        TraveledBackwordsProcent = SegmentLength(KartT.position.x, KartT.position.z, Cut2X, Cut2Z) / CutLength;

        LineLength = SegmentLength(Cn.position.x, Cn.position.z, CNn.position.x, CNn.position.z);
        PasedLineLength = SegmentLength(CPn.position.x, CPn.position.z, Cn.position.x, Cn.position.z);
    }

    private float SegmentLength(float oneX, float oneZ, float twoX, float twoZ)
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
