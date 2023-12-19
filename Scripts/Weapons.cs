using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapons : MonoBehaviour
{

    [Header("=== Weapons ===")]
    //[SerializeField] GameObject[] weapons;
    [SerializeField] PlayerStats stats = null;
    [SerializeField] static int weaponCount = 0;
    public bool[] shooting =  new bool[weaponCount];
    public bool launch = false;
    private GameObject uiCanvas;


    public Vector3 worldPos;
    public Vector3 mousePos;

    [SerializeField] GameObject reticle = null;
    [SerializeField] GameObject rangePrefab = null;
    [SerializeField] GameObject aimPrefab = null;
    [SerializeField] GameObject rangeArcPrefab = null;
    [SerializeField] GameObject weaponSlotPrefab = null;
    [SerializeField] GameObject weaponUIPrefab = null;
    [SerializeField] GameObject[] weaponSlots = null;
    [SerializeField] GameObject[] ranges;
    [SerializeField] GameObject[] weaponAim;
    [SerializeField] GameObject[] weaponList = null;
    [SerializeField] GameObject[] weaponUI = null;
    //[SerializeField] GameObject[] rangeFields;
    [SerializeField] GameObject[] rangeArcs;
    [SerializeField] GameObject Airship;
    [SerializeField] GameObject missileManager;
    public float angle;
    public float adjustedAngle;
    public float debug;

    Color aimColor1 = new Color(0.1f, 0.8f, 0f, 0.4f);
    Color aimColor2 = new Color(0.6f, 0.8f, 0f, 0.1f);
    Color aimColor3 = new Color(0.9f, 0.2f, 0f, 0.4f);
    Color aimColor4 = new Color(0.9f, 0.2f, 0f, 0.1f);

    private void Awake() {
        uiCanvas = GameObject.Find("Canvas");
        //Debug.Log(uiCanvas);
    }

    // Start is called before the first frame update
    void Start() {
        weaponCount = stats.weaponCount;
        int len = stats.ship.groups.Length;

        ranges = new GameObject[2*len];
        //rangeFields = new GameObject[len];
        weaponList = new GameObject[weaponCount];
        rangeArcs = new GameObject[len];
        weaponAim = new GameObject[weaponCount];
        weaponUI = new GameObject[weaponCount];
        float[] angles = new float[weaponCount];

        if (stats.missile != null) missileManager.GetComponent<MissileManager>().SetMissile(stats.missile);

        //Debug.Log(shooting[0]);
        for (int i = 0; i < 2*len; i++) {
            ranges[i] = Instantiate(rangePrefab, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);
            if (i < len) {
                //rangeFields[i] = Instantiate(rangeFieldPrefab, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);
                rangeArcs[i] = Instantiate(rangeArcPrefab, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);
            }
            //ranges[i+1] = Instantiate(rangePrefab, new Vector3(0,0,0), Quaternion.identity);
        }
        weaponSlots = new GameObject[weaponCount];
        foreach (baseShip.firingGroup group in stats.ship.groups) {
            foreach (int i in group.weaponsInGroup) {
                angles[i] = (group.angles[1] + group.angles[0]) / 2;
            }
        }
        for (int i = 0; i < weaponCount; i++) {
            weaponSlots[i] = Instantiate(weaponSlotPrefab, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);

            GameObject weapon = Instantiate(stats.weapons[i], stats.ship.weaponPos[i], Quaternion.identity, weaponSlots[i].transform);
            string tempName = weapon.name.ToString();
            weaponList[i] = weapon;
            weapon.GetComponent<statWeapons>().setWeaponStats(tempName.Remove(tempName.Length-7));
            weaponSlots[i].name = "weaponSlot" + i.ToString();
            weaponSlots[i].GetComponent<WeaponManager>().SetWeapon(i, weapon);
            //Debug.Log(weapon.name.ToString());
            //weaponSlots[i].GetComponent<WeaponManager>().weaponName = weapon.name;
            weaponAim[i] = Instantiate(aimPrefab, weapon.transform.position, Quaternion.identity, weapon.transform);

            weapon.transform.rotation *=  Quaternion.Euler(0,angles[i],0);
            weaponUI[i] = Instantiate(weaponUIPrefab, new Vector3(0,0,0), new Quaternion(0,0,0,0), uiCanvas.transform);
            weaponUI[i].transform.localPosition = new Vector3(-80+(80/weaponCount) + (160/weaponCount)*i, (-Screen.height/2)+50f, 0f);
            //Debug.Log(weaponUI[i].transform.localPosition);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleFiring();
        HandleAiming();
        HandleMissile();
    }

    void HandleAiming() {
        //debug = Airship.transform.localRotation.eulerAngles.y;
        Transform airshipT = Airship.transform;
        //// MOUSE RETICLE////
        //mousePos = Mouse.current.position.ReadValue();
        //Debug.Log(mousePos);
        Plane plane = new Plane(Vector3.up, 0);
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        angle = 0;
        Vector3 aimDirection = new Vector3(0,0,0);

        if (plane.Raycast(ray, out distance)) {
            worldPos = ray.GetPoint(distance);
            //Debug.Log(distance);
            //aimTransform.transform.position = worldPosition;
            //Vector3 aimDirection = (worldPos - aimTransform.position).normalized;
            aimDirection = (worldPos - transform.position).normalized;
            //Debug.DrawLine(aimTransform.position, worldPos, Color.red, Mathf.Infinity);
            //Debug.Log(aimDirection);
            angle = Mathf.Atan2(aimDirection.x, aimDirection.z) * Mathf.Rad2Deg;
            //Debug.Log(angle);
            //aimTransform.eulerAngles = new Vector3(0,angle,0);
        }
        LineRenderer lr = reticle.GetComponent<LineRenderer>();
        lr.SetPosition(0, new Vector3(transform.position.x, reticle.transform.position.y, transform.position.z));
        lr.SetPosition(1, new Vector3(worldPos.x, reticle.transform.position.y, worldPos.z));
        //adjustedAngle = angle - Airship.transform.rotation.y;
        adjustedAngle = angle - Airship.transform.localRotation.eulerAngles.y;

        //adjustedAngle = angle - (Mathf.Repeat(Airship.transform.localRotation.eulerAngles.y + 180f,360f) - 180f);
        //if (adjustedAngle > 180f) adjustedAngle -= 180f;
        //if (adjustedAngle < -180f) adjustedAngle += 180f;
        debug = Mathf.Repeat(Airship.transform.localRotation.eulerAngles.y + 180f,360f) - 180f;
        //foreach (GameObject weapon in weaponList) {
        foreach (baseShip.firingGroup group in stats.ship.groups) {
            float centerAngle = (group.angles[1] + group.angles[0]) / 2;
            float diff = Mathf.Abs(Mathf.DeltaAngle(group.angles[1], group.angles[0]));
            //Debug.Log(diff);
            if (Mathf.Abs(Mathf.DeltaAngle(adjustedAngle, centerAngle)) <= diff/2) {
                foreach (int m in group.weaponsInGroup) {
                    GameObject weapon = weaponList[m];
                    float moveSpeed = weaponSlots[m].GetComponent<WeaponManager>().rotateSpeed;
                    //Vector3 worldPosY = new Vector3(0,worldPos.y, 0);
                    //aimDirection.y = Mathf.Clamp(weapon.transform.rotation.y, group.angles[1]*Mathf.Deg2Rad, group.angles[0]*Mathf.Deg2Rad);
                    Vector3 dir = Vector3.RotateTowards(
                        weapon.transform.forward, aimDirection, moveSpeed * Time.deltaTime, 0.0f
                    );
                    LineRenderer aimLine = weaponAim[m].GetComponent<LineRenderer>();
                    WeaponManager cooldownWatch = weaponSlots[m].GetComponent<WeaponManager>();
                    if (cooldownWatch.cooldown < cooldownWatch.maxCooldown) {
                        aimLine.startColor = aimColor3;
                    } else {
                        aimLine.startColor = aimColor1;
                    }
                    //Debug.DrawRay(weapon.transform.position, dir, Color.red);
                    //Vector3 dir = new Vector3(weapon.transform.rotation.eulerAngles.x, angle, weapon.transform.rotation.eulerAngles.z);
                    //Quaternion target = Quaternion.Euler(dir);
                    //weapon.transform.rotation = Quaternion.RotateTowards(weapon.transform.rotation, target, moveSpeed*Time.deltaTime);

                    weapon.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
                    //float rotY = Mathf.Clamp(weapon.transform.rotation.y, group.angles[1]*Mathf.Deg2Rad, group.angles[0]*Mathf.Deg2Rad);
                    //weapon.transform.rotation *= Quaternion.Euler(rotY, weapon.transform.rotation.eulerAngles.y, 0);
                }
            } else {
                foreach (int m in group.weaponsInGroup) {
                    LineRenderer aimLine = weaponAim[m].GetComponent<LineRenderer>();
                    WeaponManager cooldownWatch = weaponSlots[m].GetComponent<WeaponManager>();
                    if (cooldownWatch.cooldown < cooldownWatch.maxCooldown) {
                        aimLine.startColor = aimColor4;
                    } else {
                        aimLine.startColor = aimColor2;
                    }
                }
            }

            //LineRenderer wAim = weaponAim[m].GetComponent<LineRenderer>();
            //Vector3 weaPos = weapon.transform.position;
            //lr.SetPosition(0, new Vector3(weaPos.x, reticle.transform.position.y, weaPos.z));
            //lr.SetPosition(1, new Vector3(dir.x, reticle.transform.position.y, weaPos.z));

            /*if (weapon.transform.rotation.y != angle) {
                weapon.transform.rotation = Quaternion.Slerp(weapon.transform.rotation, Quaternion.Euler(0,0,angle), moveSpeed * Time.time);
            }
            //while(weapon.transform.rotation.y > angle)
            weapon.transform.rotation = Quaternion.Euler(0,angle,0);*/
            //m++;
            //Debug.Log(weapon.name.ToString());
            //weaponSlots[i].GetComponent<WeaponManager>().weaponName = weapon.name;
        }

        //// FIRING GROUP RANGES + ARCS ////
        //Debug.Log(stats.ship.thrust);
        int i = 0;
        int j = 0;
        foreach (baseShip.firingGroup group in stats.ship.groups) {
            LineRenderer range1 = ranges[i].GetComponent<LineRenderer>();
            LineRenderer range2 = ranges[i+1].GetComponent<LineRenderer>();


            ranges[i].transform.position = airshipT.position;
            ranges[i+1].transform.position =  airshipT.position;
            ranges[i].transform.rotation =  airshipT.rotation;
            ranges[i+1].transform.rotation =  airshipT.rotation;
            range1.transform.eulerAngles += new Vector3(0,group.angles[0],0);
            range2.transform.eulerAngles += new Vector3(0,group.angles[1],0);

            //Vector3 point1 = RotatePoint(range1.GetPosition(1), new Vector3(0,0,0), new Vector3(0,group.angles[0],0));
            //Vector3 point2 = RotatePoint(range2.GetPosition(1), new Vector3(0,0,0), new Vector3(0,group.angles[1],0));
            //rangeFields[j].GetComponent<drawMesh>().SetVertices(new Vector3(0,0,0), point1, point2);

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

            j++;
            i += 2;
        }
    }

    public Vector3 RotatePoint(Vector3 point, Vector3 pivot, Vector3 angles) {
        return Quaternion.Euler(angles) * (point-pivot) + pivot;
    }

    void HandleFiring() {
        for (int i = 0; i < weaponCount; i++) {
            WeaponManager weaponManager = weaponSlots[i].GetComponent<WeaponManager>();
            weaponManager.Check(shooting[i], Airship.GetComponent<Airship>());
            weaponUI[i].GetComponent<WeaponBar>().SetMaxCooldown(weaponManager.maxCooldown);
            weaponUI[i].GetComponent<WeaponBar>().SetCooldown(weaponManager.cooldown);
        }
            //weapon.GetComponent<ParticleSystem>().Stop();
            //var emissionModule = weapon.GetComponent<ParticleSystem>();
            //if (state) emissionModule.Play();
            //else emissionModule.Stop();
    }

    void HandleMissile() {
        missileManager.GetComponent<MissileManager>().Check(launch);
    }

    #region Input Methods

    public void OnShootAll(InputAction.CallbackContext context) {
        if (context.performed) {
            foreach (baseShip.firingGroup group in stats.ship.groups) {
                float centerAngle = (group.angles[1] + group.angles[0]) / 2;
                float diff = Mathf.Abs(Mathf.DeltaAngle(group.angles[1], group.angles[0]));
                if (Mathf.Abs(Mathf.DeltaAngle(adjustedAngle, centerAngle)) <= diff/2) {
                    foreach (int m in group.weaponsInGroup) {
                        shooting[m] = true;
                    }
                }
            }
        } else {
            for(int m = 0; m < weaponCount; m++) {
                shooting[m] = false;
            }
        }
    }

    public void OnShoot1(InputAction.CallbackContext context) {
        shooting[0] = context.performed;
    }

    public void OnShoot2(InputAction.CallbackContext context) {
        shooting[1] = context.performed;
    }

    public void OnShoot3(InputAction.CallbackContext context) {
        shooting[2] = context.performed;
    }

    public void OnShoot4(InputAction.CallbackContext context) {
        shooting[3] = context.performed;
    }

    public void Aim(InputAction.CallbackContext context) {
        mousePos = context.ReadValue<Vector2>();
    }

    public void Missile(InputAction.CallbackContext context) {
        if (stats.missile != null) launch = context.performed;;
    }


    #endregion
}
