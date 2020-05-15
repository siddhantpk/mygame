using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    Health target= null;
    [SerializeField] float speed= 2;
    float damage=0;
    [SerializeField] bool isHoming=true;
    [SerializeField] GameObject hitEffect=null;
    [SerializeField] float maxLifetime=10;
    [SerializeField] GameObject[] onHitDestroy;
    [SerializeField] float LifeAfterDestroy=2;
    GameObject instigator= null;
    [SerializeField] UnityEvent OnHit;

    private void Start() {
        transform.LookAt(GetAimLocation());
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        LookAt();
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void LookAt()
    {
        if(isHoming && !target.IsDead()){
                transform.LookAt(GetAimLocation());
        }
        
    }

    public void SetTarget(Health target,GameObject instigator, float damage)
    {
        this.target= target;
        this.damage= damage;
        this.instigator= instigator;
        Destroy(gameObject, maxLifetime);
    }
    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetcapsule= target.GetComponent<CapsuleCollider>();
        if(targetcapsule==null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetcapsule.height/2;
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.GetComponent<Health>() != target) return;
        if(target.IsDead()) return;
        target.TakeDamage(instigator, damage);
        speed=0;
        OnHit.Invoke();
        if(hitEffect!=null)
        {

            Instantiate(hitEffect, GetAimLocation(), transform.rotation);
        }
        foreach(GameObject onhit in onHitDestroy)
        {
            Destroy(onhit);
        }
        Destroy(gameObject,LifeAfterDestroy);   
    }
}
