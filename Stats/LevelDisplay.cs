using UnityEngine;
using UnityEngine.UI;


namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour 
    {
        BaseStats level;
        private void Awake() {
            level= GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }
        private void Update() 
        {
            GetComponent<Text>().text= string.Format("{0,0}",level.CalculateLevel());     
        }

    }
}