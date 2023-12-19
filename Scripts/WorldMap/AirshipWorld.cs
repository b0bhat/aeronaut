using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AirshipWorld : MonoBehaviour
{
    [SerializeField] GameObject linePreFab = null;
    [SerializeField] GameObject circlePreFab = null;
    [SerializeField] GameObject terrain = null;
    [SerializeField] GameObject worldSim = null;
    public Material mat;
    public Transform[] cities = null;
    public GameObject line = null;
    public GameObject targetLine = null;
    public Vector3 worldPos;
    public Vector3 mousePos;
    public Vector3 curPos;
    [SerializeField] GameObject MoveLoc;
    public bool clicking;

    public bool tracking = false;
    public GameObject tracker = null;

    public LayerMask mask;
    public float hover = 8;
    public float moveSpeed = 5;
    public float rotateSpeed = 2;

    #region Singleton

    public static AirshipWorld instance;
    void Awake() {
        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Terrain");
        int childCount = worldSim.transform.childCount;
        cities = new Transform[childCount];
        // Cities
        for (int i = 0; i < childCount;i++) {
            cities[i] = worldSim.transform.GetChild(i);
        }
        // Airship circle
        line = Instantiate(circlePreFab, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);
        LineRenderer larc = line.GetComponent<LineRenderer>();
        larc.material = mat;
        MakeCircle(larc, 6, 30, 0);

        // Line from Airship to Loc
        targetLine = Instantiate(linePreFab, new Vector3(0,0,0), Quaternion.identity);
        targetLine.GetComponent<LineRenderer>().material = mat;

        // Loc Circle
        GameObject MoveLine = Instantiate(circlePreFab, new Vector3(0,0,0), Quaternion.identity, MoveLoc.transform);
        MoveLine.GetComponent<LineRenderer>().material = mat;
        MakeCircle(MoveLine.GetComponentInParent<LineRenderer>(), 3, 20,0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (clicking) {
            mousePos = curPos;
            SetDestination();
            MoveLoc.transform.position = worldPos;
        }
        if (tracking) {
            followTracker();
        }
        CheckDown(transform);
        Move();
        CheckCity();
        targetLine.GetComponent<LineRenderer>().SetPosition(0, transform.position);
        targetLine.GetComponent<LineRenderer>().SetPosition(1, MoveLoc.transform.position);
        //if (Mathf.Abs(transform.position.y) >=0) transform.position = new Vector3(transform.position.x,0,transform.position.z);
            
    }

    void CheckDown(Transform check) {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(check.position, Vector3.down, out hit)) {
            if (hit.distance != hover) {
                float diff = hit.distance - hover;
                check.position = new Vector3(check.position.x, check.position.y -diff, check.position.z);
            }
        } 
    }

    void followTracker() {
        if (tracker != null) {
            MoveLoc.transform.position = tracker.transform.position;
        } else {
            Debug.Log("ahhhh");
        }
    }

    public void MakeCircle(LineRenderer larc, float radius, int seg, float offset) {
        float angleFirst = 0;
        List<Vector3> arcPoints = new List<Vector3>();
        for (int k = 0; k < seg; k++) {
            float x = Mathf.Sin(Mathf.Deg2Rad * angleFirst) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angleFirst) * radius;

            arcPoints.Add(new Vector3(x,gameObject.transform.position.y,y-offset));

            angleFirst += (360 / seg+1);
        } 
        larc.positionCount = seg;
        Vector3[] arcPoints2 = arcPoints.ToArray();
        larc.SetPositions(arcPoints2);
    }

    public Vector3 zeroY(Vector3 pos) {
        pos.y = 0;
        return pos;
    }

    public void Move() {
        Vector3 target = new Vector3(MoveLoc.transform.position.x, 0, MoveLoc.transform.position.z);
        Vector3 origin = zeroY(transform.position);
        float distance = Vector3.Distance(target, origin);
        if (distance >= 0.1f) {
            transform.forward = Vector3.RotateTowards(
                transform.forward, target-origin, rotateSpeed * Time.deltaTime, 0.0f
            ); transform.Translate(Vector3.forward * Mathf.Min(moveSpeed,Mathf.Max(distance,1)) * Time.deltaTime);
        }

    }

    public void SetDestination() {
        tracking = false;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (terrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity)) {
            float terrainY = Terrain.activeTerrain.SampleHeight(MoveLoc.transform.position);
            worldPos = new Vector3(hit.point.x, terrainY+terrain.transform.position.y+hover, hit.point.z);
        }
        
        foreach(GameObject fleet in worldSim.GetComponent<worldSimulation>().fleetList) {
            if (Vector3.Distance(worldPos, fleet.transform.position) <= 8) {
                Debug.DrawLine(worldPos, fleet.transform.position);
                //Debug.Log(fleet);
                tracking = true;
                tracker = fleet;
                //MoveLoc.transform.position = fleet.transform.position;
            }
        }
        foreach(GameObject city in worldSim.GetComponent<worldSimulation>().cityList) {
            if (Vector3.Distance(worldPos, city.transform.position) <= 15) {
                Debug.Log(Vector3.Distance(worldPos, city.transform.position));
                tracking = true;
                tracker = city;
            }
        }
        
    }

    void CheckCity() {
        foreach (Transform city in cities) {
            if (Vector3.Distance(gameObject.transform.position, city.position) <= 14) {
                //enter city!
            }

        }
    }

    #region Input Methods

    public void Aim(InputAction.CallbackContext context) {
        curPos = context.ReadValue<Vector2>();
    }

    public void Click(InputAction.CallbackContext context) {
        clicking = context.performed;
    }

    #endregion

}
