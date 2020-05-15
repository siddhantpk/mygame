using System;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
public class Portal : MonoBehaviour
{
    
    [SerializeField] int ScenetoLoad;
    [SerializeField] Transform spawnPoint;
    [SerializeField] DestinationIdentifier destination;
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float fadeWaitTime = 0.5f;

    enum DestinationIdentifier
    {
      A,B,C,D,E
    }
    private void OnTriggerEnter(Collider other) 
    {
      if(other.tag=="Player")
      {
          StartCoroutine(Transition());
      }  

    }
    private IEnumerator Transition()
    {
        if(ScenetoLoad<0)
        {
          Debug.LogError("Scene to load not set");
          yield break;
        }
        Fader fader= FindObjectOfType<Fader>();
        
        DontDestroyOnLoad(gameObject);

        yield return fader.FadeOut(fadeOutTime);
        
        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        wrapper.Save();
        
        yield return SceneManager.LoadSceneAsync(ScenetoLoad);
        SavingWrapper warpper = FindObjectOfType<SavingWrapper>();
        warpper.Load();
        
        
        Portal otherPortal= GetOtherPortal();
        UpdatePlayer(otherPortal);
        
        
        yield return new WaitForSeconds(fadeWaitTime);
        yield return fader.FadeIn(fadeInTime);
        Destroy(gameObject);
    }

        private void UpdatePlayer(Portal otherPortal)
        {

           GameObject player= GameObject.FindWithTag("Player");
           player.GetComponent<NavMeshAgent>().enabled=false;
           player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
           player.transform.rotation = spawnPoint.transform.rotation;
           player.GetComponent<NavMeshAgent>().enabled=true;

        }

        private Portal GetOtherPortal()
        {
            foreach( Portal portal in FindObjectsOfType<Portal>())
            {
              if(portal==this) continue;
              if(portal.destination != destination) continue;

              return portal;
            }return null;
        }
    }
}