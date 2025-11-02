using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Скриптерсы
{
    public class PlayerSounds: MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private UnityEngine.CharacterController unityCharacterController;
        private TimedInvoker stepSoundInvoker;
        
        [Header("Settings")]
        [SerializeField] private float footstepInterval = 1f;
        [SerializeField] private float minStepSpeed = 1f;
        
        private void PlayFootStepSound()
        {
            // ChangeSurface();
            Debug.Log("звук");
            RuntimeManager.PlayOneShot("event:/SFX/InGame/Player/p_Footsteps");
        }
        
        private void Awake()
        {
            ChangeSurface();
            stepSoundInvoker = new TimedInvoker(PlayFootStepSound, footstepInterval);
        }
        
        private void Update()
        {
            var horizontalVelocity = unityCharacterController.velocity;
            horizontalVelocity = new Vector3(horizontalVelocity.x, 0, horizontalVelocity.z);

            float horizontalSpeed = horizontalVelocity.magnitude;
        
            if (horizontalSpeed < 0.1)
            {
                stepSoundInvoker.SetInterval(footstepInterval);
            }
        
            if (horizontalSpeed >= minStepSpeed && unityCharacterController.isGrounded)
            {
                stepSoundInvoker.Tick();
            }
        }

        private void ChangeSurface()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
            {
                Enum.TryParse(hit.collider.tag, out SurfaceType myStatus);
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PlayerGroundMaterial", (int)myStatus);
            }
        }
    }

    public enum SurfaceType
    {
        Dirt = 1,
        Wood = 2,
        Stone = 3, 
        Grass = 4,
        Gravel = 5
    }
    
    public class TimedInvoker
    {
        private float interval;
        private float nextInvokeTime;
        private Action action;

        public TimedInvoker(Action action, float interval)
        {
            this.action = action;
            this.interval = interval;
            this.nextInvokeTime = Time.time + interval;
        }

        public void Tick()
        {
            if (Time.time >= nextInvokeTime)
            {
                action?.Invoke();
                nextInvokeTime = Time.time + interval;
            }
        }

        public void ResetTimer()
        {
            nextInvokeTime = Time.time + interval;
        }

        public void SetInterval(float newInterval)
        {
            interval = newInterval;
        }
    }
}