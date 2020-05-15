using System;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
{
    [Range(1,99)]
    [SerializeField] int StartingLevel=1;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] Progression progression;
    [SerializeField] GameObject particleEffect=null;
    public event Action onLevelUp;
 
    int currentlevel=-1;
    private void Start() {
        currentlevel= CalculateLevel();
        Experience experience= GetComponent<Experience>();
        if(experience !=null)
        {
            experience.onExperienceGained +=UpdateLevel;
        }

    }
    private void Update() {
        
    }

    private void UpdateLevel() 
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentlevel)
            {
                currentlevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(particleEffect, transform);
        }

        private int GetLevel()
    {
        if(currentlevel<1)
        {
            currentlevel=CalculateLevel();
        }
        return currentlevel;
    }
    public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat))*(1+(GetPercentageModifier(stat)/100));
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
    {
        float total=0;
        foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
        {
            foreach(float modifier in provider.GetAdditiveModifier(stat))
            {
                total += modifier;
            }
        }
        return total;
    }
    private float GetPercentageModifier(Stat stat)
    {
        float total=0;
        foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
        {
            foreach(float modifier in provider.GetPercentageModifier(stat))
            {
                total += modifier;
            }
        }
        return total;
    }

        public int CalculateLevel()
    {
        Experience experience = GetComponent<Experience>();
        if (experience == null) return StartingLevel;
        float currentxp= experience.GetExperience();
        int penUltimateLevel = progression.GetLevel(Stat.ExperienceToLevelUp, characterClass);
        for (int level=1;level< penUltimateLevel; level++)
        {
            float XPtoLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
            if(XPtoLevelUp>currentxp)
            {
                    return level;
            }
        }
            return penUltimateLevel+1;
    }
    
}

}