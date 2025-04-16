using UnityEngine;

namespace SpaceVoyager
{
    public class ExplosionAnimationHandler : MonoBehaviour
    {
        private Animator animator;
        private bool hasStarted = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            if (animator != null)
            {
                // Get the length of the current animation
                AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
                if (clips.Length > 0)
                {
                    float animationLength = clips[0].length;
                    Debug.Log($"Animation length: {animationLength}");
                    
                    // Wait for animation to complete then freeze
                    Invoke("FreezeAnimation", animationLength * 1.0f); // Allow animation to fully complete
                }
            }
        }

        void FreezeAnimation()
        {
            if (animator != null && !hasStarted)
            {
                hasStarted = true;
                animator.speed = 0;
                Debug.Log("Animation frozen at last frame");
            }
        }
    }
}
