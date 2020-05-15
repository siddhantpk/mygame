using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRayCastables
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool IsRayCastable(PlayerController callingController)
        {
             CombatTarget target = transform.GetComponent<CombatTarget>();
            
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }
            
            return true;
        }
    }
}