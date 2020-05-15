using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageSpawner : MonoBehaviour
{
    [SerializeField] DamageText damageTextPrefab=null;
   
    void Start()
    {
        Spawn(11);
    }

    // Update is called once per frame
    public void Spawn(float DamageAmount)
    {
        DamageText instance= Instantiate<DamageText>(damageTextPrefab, transform);
        instance.SetValue(DamageAmount);
    }
}
}
