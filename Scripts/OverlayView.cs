using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayView : MonoBehaviour
{

    #region Singleton
    public static OverlayView instance;
    void Awake() {
        instance = this;
    }
    #endregion

    GameObject player = null;
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [SerializeField] List<LineRenderer> enemyLines = new List<LineRenderer>();
    [SerializeField] List<GameObject> missiles = new List<GameObject>();
    [SerializeField] List<GameObject> missileLines = new List<GameObject>();
    [SerializeField] GameObject circlePrefab;
    [SerializeField] Material playerMat;
    [SerializeField] Material enemyMat;
    [SerializeField] Material deadMat;

    GameObject circle;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance.transform.gameObject;
        circle = Instantiate(circlePrefab, player.transform.position, player.transform.localRotation, player.transform);
        LineRenderer plarc = circle.GetComponent<LineRenderer>();
        plarc.material = playerMat;
        Vector3 pSize = player.GetComponent<Collider>().bounds.size;
        makeShape.instance.MakeCircle(plarc, Mathf.Max(Mathf.Max(pSize.x, pSize.y), pSize.z)/2, 20, -1);

        Transform enemyHolder = GameObject.Find("Enemies").transform;
        foreach (Transform child in enemyHolder) {
            enemies.Add(child.gameObject);
            circle = Instantiate(circlePrefab, child.transform.position, child.transform.localRotation, child.transform);
            LineRenderer elarc = circle.GetComponent<LineRenderer>();
            elarc.material = enemyMat;
            enemyLines.Add(elarc);
            makeShape.instance.MakeCircle(elarc, 3f, 20, -1);
        }
    }

    void Update() {

    }

    public void DeadCircle(GameObject ship) {
        for (int i = 0; i < enemies.Count; i++) {
            if (enemies[i] == ship) {
                enemyLines[i].material = deadMat;
            }
        }
    }

    public void RotateMissile(GameObject missileLine) {
        //foreach (GameObject missileLine in missileLines) {
            //missileLine.transform.rotation = Quaternion.Euler(missileLine.transform.parent.rotation.x* -1f,missileLine.transform.parent.rotation.y* -1f,missileLine.transform.parent.rotation.z * -1f);
            missileLine.transform.LookAt(Camera.main.transform);
            //missileLine.transform.Rotate(Vector3.up, 0, Space.World);
            if (missileLine.transform.position.y < 0) {
                missileLine.GetComponent<LineRenderer>().material = deadMat;
            }
            //Debug.Log(missileLine);
        //}
    }

    public GameObject missileSign(GameObject missile) {
        missiles.Add(missile);
        GameObject triangle = Instantiate(circlePrefab, missile.transform.position, missile.transform.localRotation, gameObject.transform);
        LineRenderer mtri = triangle.GetComponent<LineRenderer>();
        missileLines.Add(triangle);
        makeShape.instance.MakeTriangle(mtri, 2);
        return triangle;
    }

    public void missileSignDelete(GameObject sign) {
        for (int i = 0; i < missileLines.Count; i++) {
            if (missileLines[i] == sign) {
                missileLines.Remove(missileLines[i]);
            }
        }
    }
}
