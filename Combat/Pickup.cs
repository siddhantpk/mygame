using System;
using System.Collections;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class Pickup : MonoBehaviour, IRayCastables
{
    [SerializeField] Weapon weapon =null;
    [SerializeField] float respawntime=5;
    [SerializeField] float healthToRestore=0;
    

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag=="Player")
            {
                WeaponPickup(other.gameObject);
            }
        }

        private void WeaponPickup(GameObject subject)
        {
            if(weapon!=null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weapon);
            }
            if(healthToRestore>0)
            {
                GetComponent<Health>().Heal(healthToRestore);
            }
            
            StartCoroutine(HideforSeconds(respawntime));
        }

        private IEnumerator HideforSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool show)
        {
            GetComponent<Collider>().enabled= show;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(show);
            }
        }

        public bool IsRayCastable(PlayerController callingController)
        {
            if(Input.GetMouseButton(0))
            {
                WeaponPickup(callingController.gameObject);
                
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }
}
