using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallShips
{
    public class ExplosionController : MonoBehaviour {

        [Tooltip("Child game objects that should be destroyed during explosion. For that 'DestroyPart(x)' will be called from animation clip.")]
        public GameObject[] removeParts;
        [Tooltip("Array of children that have animation for explosion and should explode by calling from parent animation clip.")]
        public ExplosionController[] childrenExplosion;

        /*
        [Tooltip("Main parent that should be destroyed after all explosins complete. Will call in 'DestroyMainParent' function from AnimationClip")]
        public GameObject mainaParent;
        */

        Animator animator;
        private bool hasExploded = false;
        public bool keepAlive = false; // Add this to control whether to destroy the object

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void DestroyPart(int index)
        {
            if (removeParts != null && index >= 0 && index < removeParts.Length)
                Destroy(removeParts[index]);
            else
                Debug.LogWarning("Index is out of range in DestroPart. index: " + index);
        }

        public void StartExplosion()
        {
            if (!hasExploded)
            {
                hasExploded = true;
                if (animator == null)
                    animator = GetComponent<Animator>();

                // Make sure we have a valid animator and animation
                if (animator != null && animator.runtimeAnimatorController != null)
                {
                    animator.SetBool("expl", true);
                    Debug.Log("Starting explosion animation");
                }
                else
                {
                    Debug.LogError("Animator or controller is missing!");
                }
            }
        }

        /// <summary>
        /// Call this function from animation clip in the last frame to remove GameObject.
        /// </summary>
        public void DestroyObject()
        {
            if (!keepAlive)
            {
                Destroy(gameObject);
            }
            else
            {
                // If we want to keep the object, just stop the animation
                if (animator != null)
                {
                    animator.speed = 0;
                    Debug.Log("Explosion animation frozen at last frame");
                }
            }
        }

        /// <summary>
        /// Call this function from animation clip in the last frame to remove parent GameObject.
        /// </summary>
        public void DestroyParentObject()
        {
            if (!keepAlive)
            {
                Destroy(transform.parent.gameObject);
            }
        }

        public void ChildExplosion(int index)
        {
            if (childrenExplosion != null && index >= 0 && index < childrenExplosion.Length)
                childrenExplosion[index].StartExplosion();
        }

        public void DestroyChildren()
        {
            if (removeParts != null && removeParts.Length > 0)
                foreach (GameObject child in removeParts)
                    Destroy(child);
        }
    }
}
