using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBarShow : MonoBehaviour
{
    [SerializeField] Health health=null;
    [SerializeField] RectTransform foreground=null;
    [SerializeField] Canvas rootCanvas=null;
    // Update is called once per frame
    void Update()
    {
        if(Mathf.Approximately(health.GetFraction(),0)||Mathf.Approximately(health.GetFraction(),1))
        {
            rootCanvas.enabled=false;
        }
        rootCanvas.enabled=true;
        foreground.localScale= new Vector3(health.GetFraction(),1,1);
    }
}

}