using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using System;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        
        float healthPoints=-1;
        bool isDead = false;
        [SerializeField] float healthpercentage=70f;
        [SerializeField] DamageTextShow takeDamage;
        [SerializeField] UnityEvent Ondie;
        [System.Serializable]
        public class DamageTextShow: UnityEvent<float>
        {

        }
        private void Awake() 
        {
            GetComponent<BaseStats>().onLevelUp += HealthRegenerate;
            if(healthPoints<0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
            
        }

        private void HealthRegenerate()
        {
            float newhp= GetComponent<BaseStats>().GetStat(Stat.Health) * (healthpercentage/100);
            healthPoints= Mathf.Max(healthPoints, newhp);
        }

        
        public bool IsDead()
        {
            return isDead;
        }

        public float HealthPercentage()
        {
            return 100 * (GetFraction());
        }

        public float GetFraction()
        {
            return healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        public void Heal(float healthToRestore)
        {
            healthPoints = Mathf.Min(healthPoints +healthToRestore, GetMaxHealth());
        }


        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if(healthPoints == 0)
            {
                Ondie.Invoke();
                Die();
                AwardExperience(instigator);
                
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }
        public float GetHealthPoints()
        {
            return healthPoints;
        }
        
        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void AwardExperience(GameObject instigator)
        {
            Experience experience= instigator.GetComponent<Experience>();
            if(experience==null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
         public object CaptureState()
        {
            return  healthPoints;
        }
        public void RestoreState(object state)
        {
            healthPoints= (float)state;

            if(healthPoints==0)
            {
                Die();
            }
        }

    }
}