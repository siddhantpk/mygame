using UnityEngine;

namespace RPG.Combat
{
    public class DestroyAfterEffect : MonoBehaviour 
    {
         [SerializeField] GameObject targetToDestroy=null;
        private void Update() 
        {
            if(!GetComponent<ParticleSystem>().IsAlive())
            {
                if(targetToDestroy!=null)
                {
                    Destroy(targetToDestroy);
                }
                Destroy(gameObject);
            }   
        }
    }
}