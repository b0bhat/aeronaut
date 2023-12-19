using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    [SerializeField] public float maxCooldown = 1f;
    [SerializeField] public float tickRate = 0.1f;
    [SerializeField] public int burstCount = 1;
    [SerializeField] public float burstTime = 1f;
    [SerializeField] public float rotateSpeed = 1f;
    [SerializeField] public float force = 1f;
    [SerializeField] public int weaponType = 0;
    [SerializeField] public float damage = 0;
    [SerializeField] public float range = 0;
    [SerializeField] statWeapons weapon;
    //[SerializeField] PlayerStats stats = null;
    public int weaponNum = 0;

    public float cooldown;

    public int burstNum;
    public float burstTick;

    public float firingtime;

    void Start()
    {
        
    }

    public void SetWeapon(int i, GameObject wep) {
        weaponNum = i;
        weapon = wep.GetComponent<statWeapons>();
        maxCooldown = weapon.stat_maxCooldown;
        burstCount = weapon.stat_burstCount;
        burstTime = weapon.stat_burstTime;
        rotateSpeed = weapon.stat_rotateSpeed;
        weaponType = weapon.stat_weaponType;
        force = weapon.stat_force;
        damage = weapon.stat_damage;
        range = weapon.stat_range;
        

        //Debug.Log(weapon.GetComponent<weaponStat>().stat_maxCooldown);
        //TODO update change particle weapon range
        weapon.GetComponent<BulletParticle>().SetWeaponType(weaponType);
        weapon.GetComponent<BulletParticle>().SetWeaponDamage(damage);


        cooldown = maxCooldown;
        burstNum = 0;
        burstTick = 0;
    }

    public void Check(bool shooting, Airship airship) {
        if (shooting && cooldown >= maxCooldown) {
            cooldown = 0f;
            Fire(airship);
            shooting = false;
            burstNum = 1;
        } else if (burstNum >= 1) {
            if (burstTick >= burstNum * burstTime){
                Fire(airship);
                burstNum += 1;
            } else {
                weapon.GetComponent<ParticleSystem>().Stop();
            } burstTick += tickRate;
            if (burstTick >= burstCount * burstTime) {
                burstNum = 0;
                burstTick = 0;
            }
        } else {
            if (cooldown < maxCooldown) {
                cooldown += tickRate;
                weapon.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    public void Fire(Airship airship) {
        weapon.GetComponent<ParticleSystem>().Play();
        airship.Recoil(weapon.transform, force);
        CinemachineShake.Instance.ShakeCamera(force/50, .1f);
    }
}