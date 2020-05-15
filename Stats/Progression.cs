using UnityEngine;
using System.Collections.Generic;


namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/Progression", order = 0)]
public class Progression : ScriptableObject 
{
    [SerializeField] ProgressionCharacterClass[] characterClasses=null;

    Dictionary<CharacterClass,Dictionary<Stat,float[]>> lookupTable=null;
    internal float GetStat(Stat stat, CharacterClass characterClass, int level)
    {
       BuildLookup();
       float[] levels= lookupTable[characterClass][stat];
       if(levels.Length<level) return 0;
       return levels[level-1];

    }

    public void BuildLookup()
    {
        if(lookupTable !=null) return;
        lookupTable= new Dictionary<CharacterClass,Dictionary<Stat,float[]>>();
        foreach(ProgressionCharacterClass progressionclass in characterClasses)
        {
            var StatLookupTable= new Dictionary<Stat, float[]>();
            foreach(ProgressionStat progressionStat in progressionclass.stats)
            {
                StatLookupTable[progressionStat.stats]=progressionStat.levels;
            }
            lookupTable[progressionclass.characterClass]= StatLookupTable;

        }

    }
    public int GetLevel(Stat stat, CharacterClass characterClass)
    {
        float[] levels = lookupTable[characterClass][stat];
        return levels.Length;

    }
    
    [System.Serializable]
    
    class ProgressionCharacterClass
    {
        public CharacterClass characterClass;
        public ProgressionStat[] stats;
    }

    [System.Serializable]
    class ProgressionStat
    {   
        public Stat stats;
        public float[] levels;

    }
}

}
