using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        
        [SerializeField] float timeBetweenAttacks = 1f;
        
        [SerializeField] Weapon defaultWeapon= null;
        [SerializeField] Transform rightHand = null;
        [SerializeField] Transform leftHand = null;
        Weapon currentWeapon;
        

        


        Health target;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Start() 
        {
            if(currentWeapon==null)
            {
                EquipWeapon(defaultWeapon);
            }
            
            
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator= GetComponent<Animator>();
            if (!animator) Debug.Log($"{name} has no Animator");
            if (!weapon) Debug.Log($"{name} is trying to equip a null weapon");
            if(!rightHand) Debug.Log($"{name}'s fighter component does not have the hand transform set");
            weapon.Spawn(rightHand,leftHand, animator);
        }


        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        void Hit()
        {
            float damage = GetComponent<BaseStats>().GetStat(Stat.damage);
            if(target == null) { return; }
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHand, leftHand, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        void Shoot()
        {
            Hit();
        }
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public Health GetTarget()
        {
            return target;
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if(stat== Stat.damage)
            {
                yield return currentWeapon.TakeDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if(stat== Stat.damage)
            {
                yield return currentWeapon.AttackPercentage();
            }   
        }
        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName= (string)state;
            Weapon currentWeapon= UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(currentWeapon);
        }
    }
}