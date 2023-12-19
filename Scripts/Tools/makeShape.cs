using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeShape : MonoBehaviour
{
    #region Singleton
    public static makeShape instance;
    void Awake() {
        instance = this;
    }
    #endregion

    public void MakeCircle(LineRenderer larc, float radius, int seg, float offset) {
        float angleFirst = 0;
        List<Vector3> arcPoints = new List<Vector3>();
        for (int k = 0; k < seg; k++) {
            float x = Mathf.Sin(Mathf.Deg2Rad * angleFirst) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angleFirst) * radius;

            arcPoints.Add(new Vector3(x,larc.gameObject.transform.position.y+offset,y));

            angleFirst += (360 / seg+1);
        }
        larc.positionCount = seg;
        Vector3[] arcPoints2 = arcPoints.ToArray();
        larc.SetPositions(arcPoints2);
    }

    public void MakeTriangle(LineRenderer larc, float radius){
        larc.positionCount = 4;
        List<Vector3> triPoints = new List<Vector3>();
        triPoints.Add(new Vector3(0,radius,0));
        triPoints.Add(new Vector3(radius,-radius,0));
        triPoints.Add(new Vector3(-radius,-radius,0));
        triPoints.Add(new Vector3(0,radius,0));
        Vector3[] triPoints2 = triPoints.ToArray();
        larc.SetPositions(triPoints2);
    }

}
