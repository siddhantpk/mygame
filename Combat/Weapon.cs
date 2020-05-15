using System;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{

    
     
     [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Make new weapon", order = 0)]
     public class Weapon : ScriptableObject 
     {
        [SerializeField] GameObject equippedPrefab= null;
        [SerializeField] AnimatorOverrideController animatorOverrideController=null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRighthand=true;
        [SerializeField] Projectile projectile;
        [SerializeField] float percentbonus=5;
        const string WeaponName="Weapon";
        

        public void Spawn(Transform rightHand,Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand,leftHand);
            if(equippedPrefab!=null)
            {
                Transform hand = GetTransform(rightHand, leftHand);
                GameObject weapon=  Instantiate(equippedPrefab, hand);
                weapon.name= WeaponName;
            }
            if (animator!=null)
            {
                animator.runtimeAnimatorController= animatorOverrideController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon= rightHand.Find(WeaponName);
            if(oldWeapon==null)
            {
                oldWeapon= leftHand.Find(WeaponName);
            }
            if(oldWeapon==null) return;
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform hand;
            if (isRighthand) hand = rightHand;
            else hand = leftHand;
            return hand;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float damage)
        {
            Projectile projectileInstance= Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target,instigator, damage);
        }
        public float TakeDamage()
        {
            return weaponDamage;
        }
        public float AttackPercentage()
        {
            return percentbonus;
        }
        public float GetRange()
        {
            return weaponRange;
        }
     }
    
}