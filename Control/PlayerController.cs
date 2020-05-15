using UnityEngine.EventSystems;
using RPG.Combat;
using RPG.Attributes;
using RPG.Movement;
using UnityEngine;
using System;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
       
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Vector2 hotspot;
            public Texture2D texture;
        }

        [SerializeField] CursorMapping[] cursorMappings=null;
        [SerializeField] float NavMeshProjectionDistance=1f;
        [SerializeField] float maxNavLength = 40f;

        private void Start() {
            health = GetComponent<Health>();
        }


        private void Update()
        {
            if (health.IsDead())
            {
                SetCursorType(CursorType.None);
                return;
            }
            if(InteractWithComponent()) return;
            if(InteractWithUI()) return;
            if (InteractWithMovement()) return;
            SetCursorType(CursorType.None);
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit= RayCastNavMesh(out target);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target);
                }
                SetCursorType(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RayCastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if(!hasHit) return false;
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh= NavMesh.SamplePosition(hit.point,out navMeshHit, NavMeshProjectionDistance, NavMesh.AllAreas);
            if(!hasCastToNavMesh) return false;
            target = navMeshHit.position; 
            // NavMeshPath navMeshPath= new NavMeshPath();
            // bool hasPath= NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, navMeshPath);
            // if(!hasPath) return false;
            // if(navMeshPath.status != NavMeshPathStatus.PathComplete) return false;
            // if (GetPathLength(navMeshPath)<maxNavLength) return false;
            return true;
        }

        private float GetPathLength(NavMeshPath navMeshPath)
        {
            float total=0;
            if(navMeshPath.corners.Length <2) return total;
            for(int i=0; i<navMeshPath.corners.Length-1; i++)
            {
                total+=Vector3.Distance(navMeshPath.corners[i], navMeshPath.corners[i+1]);
            }
            return total;
        }

        private bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                SetCursorType(CursorType.UI);
                return true;
            }
            return false;
        }

        public bool InteractWithComponent()
        {
            RaycastHit[] hits = GetHits();
            foreach (RaycastHit hit in hits)
            {
                IRayCastables[] rayCastables = hit.transform.GetComponents<IRayCastables>();
                foreach (IRayCastables rayCastable in rayCastables)
                {
                    if (rayCastable.IsRayCastable(this))
                    {
                        SetCursorType(CursorType.Combat);
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] GetHits()
        {
            RaycastHit[] hits=Physics.RaycastAll(GetMouseRay());
            float[] distances= new float[hits.Length];
            for(int i=0;i<hits.Length;i++)
            {
                distances[i]=hits[i].distance;
            }
            Array.Sort(distances,hits);
            return hits;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        private void SetCursorType(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }
        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if(mapping.type==type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }
    }
}