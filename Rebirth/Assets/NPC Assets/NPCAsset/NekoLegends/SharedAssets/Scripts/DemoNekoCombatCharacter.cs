
using System;
using UnityEngine;


namespace NekoLegends
{
    public class DemoNekoCombatCharacter : MonoBehaviour
    {
        [SerializeField] protected AnimationClip[] animations;
        [SerializeField] protected GameObject leftHandItem;
        [SerializeField] protected GameObject rightHandItem;

        protected int _currentAnimationIndex = -1;
        protected float _transitionDuration = 0.05f;
        protected string _currentEventName;

        protected GameObject _currentAura;



        public Animator animator { get; protected set; }

        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
            
        }

        //used for when we have a specific set of animations playing in order an looping back
        public void NextAnimation() 
        {
            _currentAnimationIndex++;
            if (_currentAnimationIndex >= animations.Length)
            {
                _currentAnimationIndex = 0;
            }
            AnimationClip nextAnimation = animations[_currentAnimationIndex];
            this.animator.CrossFade(nextAnimation.name, _transitionDuration);
            DemoScenes.Instance.DescriptionText.SetText("Animation Animation: " + nextAnimation.name);
        }

        public virtual void PlayAnimation(string animationName)
        {
            this.animator.CrossFade(animationName, _transitionDuration);
            DemoScenes.Instance.DescriptionText.SetText("Animation Animation: " + animationName);
            
        }


        public virtual void AnimationEventHandler(string eventName){}
        protected virtual void CreateProjectile(Projectile go){ }

        protected virtual void CreateAura(GameObject go, Vector3 offset=default, Vector3 rotation = default) { }

        protected virtual void SpawnVFX(GameObject go, Vector3 offset = default,Vector3 rotation = default) { }

        protected virtual void DestroyAura()
        {
            // Destroy the previous aura if it exists
            if (_currentAura != null)
            {
                Destroy(_currentAura);
            }

        }

    }
}
