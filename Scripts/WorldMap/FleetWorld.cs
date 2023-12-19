using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetWorld : MonoBehaviour
{

    public enum FlightPlan {
        Follow,
        Guard,
        Location,
        None,
    }

    public enum FlightType {
        Trade,
        Hunt,
        Patrol,
        Pirate,
        None,
    }

    [SerializeField] GameObject linePreFab = null;
    [SerializeField] GameObject circlePreFab = null;
    [SerializeField] GameObject terrain = null;

    public List<GameObject> shipList = new List<GameObject>();

    public Vector3 Follow;
    public Vector3 Guard;
    public Vector3 Location;

    public Material mat;
    public GameObject line = null;
    public GameObject targetLine = null;
    public Vector3 worldPos;
    public Vector3 playerPos;
    //[SerializeField] GameObject MoveLoc;
    public bool clicking;
    public LayerMask mask;
    public float hover = 8;
    public float moveSpeed = 5;
    public float rotateSpeed = 2;

    public FlightType flightType;
    public FlightPlan flightPlan;

    //constructor

    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Terrain");
        // Airship circle
        line = Instantiate(circlePreFab, gameObject.transform.position, gameObject.transform.localRotation, gameObject.transform);
        LineRenderer larc = line.GetComponent<LineRenderer>();
        larc.material = mat;
        //makeShape.instance.MakeCircle(larc, 6, 30, 0);

        // Line from Airship to Loc
        //targetLine = Instantiate(linePreFab, new Vector3(0,0,0), Quaternion.identity);
        //targetLine.GetComponent<LineRenderer>().material = mat;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDown(transform);
        playerPos = AirshipWorld.instance.transform.position;
        switch (flightType) {
            default:
            case FlightType.None:
                break;
            case FlightType.Patrol:
                PatrolGuard(Guard);
                break;
            case FlightType.Trade:
                MoveToLocation(Location);
                break;
        }
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

    public Vector3 zeroY(Vector3 pos) {
        pos.y = 0;
        return pos;
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


    public void MoveToLocation(Vector3 movePos) {
        Vector3 target = new Vector3(movePos.x, 0, movePos.z);
        Vector3 origin = zeroY(transform.position);
        float distance = Vector3.Distance(target, origin);
        if (distance >= 0.1f) {
            transform.forward = Vector3.RotateTowards(
                transform.forward, target-origin, rotateSpeed * Time.deltaTime, 0.0f
            ); transform.Translate(Vector3.forward * Mathf.Min(moveSpeed,Mathf.Max(distance,1)) * Time.deltaTime);
        }
        //targetLine.GetComponent<LineRenderer>().SetPosition(0, transform.position);
        //targetLine.GetComponent<LineRenderer>().SetPosition(1, movePos);

    }

    public void PatrolGuard(Vector3 movePos) {
        Vector3 target = new Vector3(movePos.x, 0, movePos.z);
        Vector3 origin = zeroY(transform.position);
        float distance = Vector3.Distance(target, origin);
        if (distance <= 30f) {
            transform.Translate(Vector3.forward * -Mathf.Min(moveSpeed,Mathf.Max(distance,1)) * Time.deltaTime);
        } else {
            transform.RotateAround(target, Vector3.up, 10*Time.deltaTime);
        }
        //targetLine.GetComponent<LineRenderer>().SetPosition(0, transform.position);
        //targetLine.GetComponent<LineRenderer>().SetPosition(1, movePos);

    }

    public void setLocation(Vector3 setLocation) {
        Location = setLocation;
        //flightPlan = FlightPlan.Location;
    }

    public void setGuard(Vector3 setGuard) {
        Guard = setGuard;
        //flightPlan = FlightPlan.Patrol;
    }

    public void setFollow(Vector3 setFollow) {
        Follow = setFollow;
        //flightPlan = FlightPlan.Follow;
    }

    public void setShips(string owner, int importance) {
        List<stat_Nations.nationShip> statNationShips = MapLoader.instance.GetComponent<stat_Nations>().getShips("Conclave");
        int shipCount = (int) Random.Range(Mathf.Min(importance-1,1),importance+2);
        //Debug.Log(shipCount);
        int frigateCount = shipCount;
        int destroyerCount = 0;
        int cruiserCount = 0;
        int battleshipCount = 0;
        if ((importance >= 2) && ((0.4)*importance >= Random.Range(0,1))) {
            destroyerCount += Mathf.Min((int) Random.Range(0,importance), frigateCount);
            frigateCount += -destroyerCount;
        } if ((importance >= 3) && ((0.3)*importance >= Random.Range(0,1))) {
            cruiserCount += Mathf.Min((int) Random.Range(0,importance), frigateCount);
            frigateCount += -cruiserCount;
        } if ((importance >= 4) && ((0.2)*importance >= Random.Range(0,1))) {
            battleshipCount += Mathf.Min((int) Random.Range(1,importance-1), frigateCount);
            frigateCount += -battleshipCount;
        }
        Debug.Log(importance + " " + frigateCount + destroyerCount+ cruiserCount+ battleshipCount);
        foreach (stat_Nations.nationShip ship in statNationShips) {
            if (ship.shipClass == "frigate") {
                for (int i = 0; i < frigateCount; i++) shipList.Add(ship.name);
            } else if (ship.shipClass == "destroyer") {
                for (int i = 0; i < destroyerCount; i++) shipList.Add(ship.name);
            } else if (ship.shipClass == "cruiser") {
                for (int i = 0; i < cruiserCount; i++) shipList.Add(ship.name);
            } else if (ship.shipClass == "battleship") {
                for (int i = 0; i < battleshipCount; i++) shipList.Add(ship.name);
            }
        }
    }

    #region Input Methods
/*
    public void Aim(InputAction.CallbackContext context) {
        curPos = context.ReadValue<Vector2>();
    }

    public void Click(InputAction.CallbackContext context) {
        clicking = context.performed;
    }*/

    #endregion

}

