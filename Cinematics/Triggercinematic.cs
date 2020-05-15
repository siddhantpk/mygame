using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Playables;

public class Triggercinematic : MonoBehaviour
{


    
    public class Triggercutscene : MonoBehaviour 
    {
        bool AlreadyTriggered= false;
        private void OnTriggerEnter(Collider other) 
        {
           if(!AlreadyTriggered && other.gameObject.tag=="Player")
           {
           GetComponent<PlayableDirector>().Play();
           }
        }
        
    
}
}
