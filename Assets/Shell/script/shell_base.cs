using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shell_base : MonoBehaviour
{
    public float Range = 4;
    public float Damage = 10;
    public float MinimumDamageRatio = 0.1f;
    public float ExplosionForce = 1000f;
    public GameObject ExplosionEffect;
    public AudioClip ExplosionSound, FireSound;
    void OnTriggerEnter(Collider coll)
    {
        Explosion();
    }
    void Start()
    {
        AudioSource.PlayClipAtPoint(FireSound, transform.position);
    }
    void Update()
    {
        if(transform.position.y < -5)
            Destroy(this);
    }

    void Explosion()
    {
        Collider[] coll = Physics.OverlapSphere(transform.position, Range);
        GameObject p = Instantiate(ExplosionEffect, transform.position, transform.rotation);
        p.GetComponent<ParticleSystem>().Play();
        AudioSource.PlayClipAtPoint(ExplosionSound, transform.position);
        HashSet<Transform> set = new HashSet<Transform>();

        foreach(var obj in coll)
        {
            if(set.Contains(obj.transform.root))
                continue;
            set.Add(obj.transform.root);
            Tank tank = obj.GetComponent<Tank>();
            if(tank != null)
                tank.AddDamage(CalculateDamge(transform.position, obj.transform));
            Rigidbody rigid = obj.GetComponent<Rigidbody>();
            if(rigid != null)
                rigid.AddExplosionForce(ExplosionForce, transform.position, Range);
        }
        Destroy(this.gameObject);
    }
    float CalculateDamge(Vector3 position, Transform other)
    {
        float dist = Vector3.Distance(position, other.position);
        if(dist >= Range)
            return Damage * MinimumDamageRatio;
        
        return Damage * (1 - dist/Range);
    }
}

