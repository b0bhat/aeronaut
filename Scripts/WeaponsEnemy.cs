using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsEnemy : MonoBehaviour
{
    [Header("=== Weapons ===")]
    //[SerializeField] GameObject[] weapons;
    [SerializeField] baseShip ships = null;
    [SerializeField] static int weaponCount = 2;
    public bool[] shooting =  new bool[weaponCount];
    private Transform aimTransform;
    private GameObject uiCanvas;

    [SerializeField] GameObject rangePrefab = null;
    [SerializeField] GameObject aimPrefab = null;
    [SerializeField] GameObject rangeArcPrefab = null;
    [SerializeField] GameObject weaponSlotPrefab = null;
    [SerializeField] GameObject[] weaponSlots = null;
    [SerializeField] GameObject[] ranges;
    [SerializeField] GameObject[] weapons;
    [SerializeField] GameObject[] weaponAim;
    [SerializeField] GameObject[] weaponList = null;
    //[SerializeField] GameObject[] rangeFields;
    [SerializeField] GameObject[] rangeArcs;
    [SerializeField] GameObject Airship;
    public float angle;
    public float adjustedAngle;
    public int len;
    baseShip ship;
    Transform target;
    bool dead = false;

    public float debug;

    Color aimColor1 = new Color(0.1f, 0.7f, 0f, 0.32f);
    Color aimColor2 = new Color(0.6f, 0.75f, 0f, 0.05f);

    private void Awake() {
        //aimTransform = transform.Find("Aim");
        //uiCanvas = GameObject.Find("Canvas");
        //Debug.Log(uiCanvas);
    }

    // Start is called before the first frame update
    void Start() {
        target = Player.instance.transform;
        ship = ships.GetComponent<baseShip>();
        weaponCount = ship.weaponNum;
        len = ship.groups.Length;

        ranges = new GameObject[2*len];
        //rangeFields = new GameObject[len];
        weaponList = new GameObject[weaponCount];
        rangeArcs = new GameObject[len];
        weaponAim = new GameObject[weaponCount];
        float[] angles = new float[weaponCount];

        //Debug.Log(shooting[0]);
        for (int i = 0; i < 2*len; i++) {
            ranges[i] = Instantiate(rangePrefab, new Vector3(0,0,0), Quaternion.identity, Airship.transform);
            if (i < len) {
                //rangeFields[i] = Instantiate(rangeFieldPrefab, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);
                rangeArcs[i] = Instantiate(rangeArcPrefab, new Vector3(0,0,0), Quaternion.identity, Airship.transform);
            }
            //ranges[i+1] = Instantiate(rangePrefab, new Vector3(0,0,0), Quaternion.identity);
        }
        weaponSlots = new GameObject[weaponCount];
        foreach (baseShip.firingGroup group in ship.groups) {
            foreach (int i in group.weaponsInGroup) {
                angles[i] = (group.angles[1] + group.angles[0]) / 2;
            }
        }
        for (int i = 0; i < weaponCount; i++) {
            weaponSlots[i] = Instantiate(weaponSlotPrefab, new Vector3(0,0,0), Quaternion.identity, Airship.transform);

            GameObject weapon = Instantiate(weapons[i], ship.weaponPos[i], Quaternion.identity, weaponSlots[i].transform);
            string tempName = weapon.name.ToString();
            weaponList[i] = weapon;
            weapon.GetComponent<statWeapons>().setWeaponStats(tempName.Remove(tempName.Length-7));
            weaponSlots[i].name = "weaponSlot" + i.ToString();
            weaponSlots[i].GetComponent<WeaponManager>().SetWeapon(i, weapon);
            //Debug.Log(weapon.name.ToString());
            //weaponSlots[i].GetComponent<WeaponManager>().weaponName = weapon.name;
            weaponAim[i] = Instantiate(aimPrefab, weapon.transform.position, Quaternion.identity, weapon.transform);

            weapon.transform.rotation *=  Quaternion.Euler(0,angles[i],0);
            //Debug.Log(weaponUI[i].transform.localPosition);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dead) {
            HandleFiring();
            HandleAiming();
        }
    }

    void HandleAiming() {

        //debug = Airship.transform.localRotation.eulerAngles.y;
        Transform airshipT = Airship.transform;
        /*
        LineRenderer lr = reticle.GetComponent<LineRenderer>();
        lr.SetPosition(0, new Vector3(transform.position.x, reticle.transform.position.y, transform.position.z));
        lr.SetPosition(1, new Vector3(worldPos.x, reticle.transform.position.y, worldPos.z));
        //adjustedAngle = angle - Airship.transform.rotation.y;
        adjustedAngle = angle - Airship.transform.localRotation.eulerAngles.y;
        */
        //adjustedAngle = angle - (Mathf.Repeat(Airship.transform.localRotation.eulerAngles.y + 180f,360f) - 180f);
        //if (adjustedAngle > 180f) adjustedAngle -= 180f;
        //if (adjustedAngle < -180f) adjustedAngle += 180f;
        //debug = Mathf.Repeat(Airship.transform.localRotation.eulerAngles.y + 180f,360f) - 180f;
        //foreach (GameObject weapon in weaponList) {
        Vector3 aimDirection = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(target.position, transform.position);
        angle = Mathf.Atan2(aimDirection.x, aimDirection.z) * Mathf.Rad2Deg;
        adjustedAngle = angle - Airship.transform.localRotation.eulerAngles.y;
        foreach (baseShip.firingGroup group in ship.groups) {
            float centerAngle = (group.angles[1] + group.angles[0]) / 2;
            float diff = Mathf.Abs(Mathf.DeltaAngle(group.angles[1], group.angles[0]));
            //Debug.Log(diff);
            if (Mathf.Abs(Mathf.DeltaAngle(adjustedAngle, centerAngle)) <= diff/2) {
                foreach (int m in group.weaponsInGroup) {
                    GameObject weapon = weaponList[m];
                    float moveSpeed = weaponSlots[m].GetComponent<WeaponManager>().rotateSpeed;
                    float aimDiff = Mathf.Abs(Mathf.DeltaAngle(adjustedAngle,  (weapon.transform.localRotation).eulerAngles.y));
                    shooting[m] = false;
                    if ((aimDiff < 10f) && (distance < weaponSlots[m].GetComponent<WeaponManager>().range)) shooting[m] = true;
                    //Vector3 worldPosY = new Vector3(0,worldPos.y, 0);
                    //aimDirection.y = Mathf.Clamp(weapon.transform.rotation.y, group.angles[1]*Mathf.Deg2Rad, group.angles[0]*Mathf.Deg2Rad);
                    Vector3 dir = Vector3.RotateTowards(
                        weapon.transform.forward, aimDirection, moveSpeed * Time.deltaTime, 0.0f
                    );
                    LineRenderer aimLine = weaponAim[m].GetComponent<LineRenderer>();
                    aimLine.startColor = aimColor1;
                    weapon.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
                }
            } else {
                foreach (int m in group.weaponsInGroup) {
                    LineRenderer aimLine = weaponAim[m].GetComponent<LineRenderer>();
                    aimLine.startColor = aimColor2;
                    shooting[m] = false;
                }
            }

        }

        //// FIRING GROUP RANGES + ARCS ////
        //Debug.Log(stats.ship.thrust);
        int i = 0;
        int j = 0;
        //Debug.DrawLine(transform.position, target.position, Color.red, Mathf.Infinity);
        foreach (GameObject slot in weaponSlots) {
            slot.transform.position = transform.position;
            slot.transform.rotation = transform.rotation;
        }
        foreach (baseShip.firingGroup group in ship.groups) {
            LineRenderer range1 = ranges[i].GetComponent<LineRenderer>();
            LineRenderer range2 = ranges[i+1].GetComponent<LineRenderer>();


            ranges[i].transform.position = airshipT.position;
            ranges[i+1].transform.position =  airshipT.position;
            ranges[i].transform.rotation =  airshipT.rotation;
            ranges[i+1].transform.rotation =  airshipT.rotation;
            range1.transform.eulerAngles += new Vector3(0,group.angles[0],0);
            range2.transform.eulerAngles += new Vector3(0,group.angles[1],0);

            List<Vector3> arcPoints = new List<Vector3>();
            float angleFirst = group.angles[0];
            float arcLength = group.angles[1] - group.angles[0];
            int seg = 10;
            for (int k = 0; k < seg; k++) {
                float x = Mathf.Sin(Mathf.Deg2Rad * angleFirst) * 5;
                float y = Mathf.Cos(Mathf.Deg2Rad * angleFirst) * 5;

                arcPoints.Add(new Vector3(x,airshipT.position.y,y));

                angleFirst += (arcLength / seg+1);
            }
            LineRenderer larc = rangeArcs[j].GetComponent<LineRenderer>();
            larc.positionCount = seg;
            Vector3[] arcPoints2 = arcPoints.ToArray();
            larc.SetPositions(arcPoints2);
            larc.transform.position = transform.position;
            larc.transform.rotation = transform.rotation;

            j++;
            i += 2;
        }
    }

    void HandleFiring() {

        for (int i = 0; i < weaponCount; i++) {
            WeaponManager weaponManager = weaponSlots[i].GetComponent<WeaponManager>();
            weaponManager.Check(shooting[i], Airship.GetComponent<Airship>());
        }
    }

    public void setDead() {
        dead = true;
        foreach (GameObject element in ranges) {
            element.SetActive(false);
        } foreach (GameObject element in rangeArcs) {
            element.SetActive(false);
        } foreach (GameObject element in weaponAim) {
            element.SetActive(false);
        }
    }

}
