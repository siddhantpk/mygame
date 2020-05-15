namespace RPG.Core
{
    using System;
    using UnityEngine;
    
    public class PersistentObjectSpawner : MonoBehaviour 
    {
        bool hasSpawned= false;
        [SerializeField] GameObject PersistentObjectPrefab;
        private void Awake() 
        {
            if(hasSpawned) return;
            
            SpawnPersistenObject();
            hasSpawned=true;
        
        }

        private void SpawnPersistenObject()
        {
            GameObject persistenObject=  Instantiate(PersistentObjectPrefab);
            DontDestroyOnLoad(persistenObject);
        }
    }
}