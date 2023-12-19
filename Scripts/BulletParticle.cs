using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BulletParticle : MonoBehaviour
{
    [SerializeField] private float damage = 20f;

    public new ParticleSystem particleSystem;
    public GameObject hit;
    List<ParticleCollisionEvent> colEvents = new List<ParticleCollisionEvent>();
    public int weaponType;

    public void SetWeaponType(int type) {
        weaponType = type;
    }

    public void SetWeaponDamage(float dmg) {
        damage = dmg;
    }

    private void OnParticleCollision(GameObject other) {
        ParticleSystem.Particle[] par = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        int events = particleSystem.GetCollisionEvents(other, colEvents);
        //Debug.Log("hit");
        for (int i = 0; i < events; i++){
            Instantiate(hit, colEvents[i].intersection, Quaternion.LookRotation(colEvents[i].normal));
            /*for (int i = 0; i < particleSystem.GetParticles(par); i++) {
                if (Vector3.Magnitude(par[i].position - colEvents[i].intersection) < 0.05f) {

                }
            }*/ //bounce off shield
        }

        if(other.TryGetComponent(out Player player)) {
            player.TakeDamage(damage);
        } else if(other.TryGetComponent(out Airship enemy)) {
            enemy.TakeDamage(damage);
        }
    }
}

