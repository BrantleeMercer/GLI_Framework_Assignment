using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GLIFramework.Scripts
{
    public class ShootGuns : MonoBehaviour
    {
        /// <summary>
        /// Amount of damage the gun will do to the target
        /// </summary>
        [field: SerializeField, Tooltip("Amount of damage the gun will do to the target"), Header("Variables")]
        public int DamageAmount { get; private set; } = 5;
        /// <summary>
        /// Reference to a bullet spark prefab
        /// </summary>
        [field: SerializeField, Tooltip("Reference to a bullet spark prefab")]
        public GameObject BulletSparkPrefab { get; private set; } = null;
        
        /// <summary>
        /// Reference to the Input Action for shooting the gun
        /// </summary>
        [field: SerializeField, Tooltip("Reference to the Input Action for shooting the gun"),
                Header("Input Action References")]
        public InputActionReference ShootGunInputReference { get; private set; } = null;

        /// <summary>
        /// LayerMask for barriers
        /// </summary>
        private LayerMask _barriers = 1 << 7;
        /// <summary>
        /// LayerMask for AI Bots
        /// </summary>
        private LayerMask _aiBot = 1 << 8;
        /// <summary>
        /// LayerMask for Exploding barrels
        /// </summary>
        private LayerMask _explodingBarrels = 1 << 9;

        public Action OnBarrierBroken;
        
        private void OnShootButtonPressed(InputAction.CallbackContext context)
        {
            if(GameManager.Instance.TotalAmmoCount <= 0)
                return;
            
            UIManager.Instance.UpdateCount(LabelName.Ammo);
            AudioManager.Instance.PlaySoundEffect(SoundFX.Gunfire);

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            
            //Using the reticule as the focal point, after clicking the shoot button, we send out a raycast.  If there is a hit and it is a barrier or ai bot we continue
            //otherwise we finish
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity,
                    _barriers | _aiBot | _explodingBarrels))
            {
                Vector3 hitPoint = hit.point; // Holds the point at which the ray hits
                if (hit.collider.tag.Equals("AI"))
                {
                    AudioManager.Instance.PlaySoundEffect(SoundFX.AIHit);
                    hit.collider.gameObject.GetComponent<MoveAIToEnd>().DamageAIBot(DamageAmount);
                }

                if (hit.collider.tag.Equals("Barrier"))
                {
                    AudioManager.Instance.PlaySoundEffect(SoundFX.BarrierHit);
                    var temp = hit.collider.gameObject.GetComponent<BarrierBehavior>();
                    temp.DamageForceField(DamageAmount);
                }
                
                if (hit.collider.tag.Equals("Barrel"))
                {
                    AudioManager.Instance.PlaySoundEffect(SoundFX.BarrierHit);
                    var temp = hit.collider.gameObject.GetComponent<ExplodingBarrelsBehavior>();
                    temp.DamageBarrel(DamageAmount);
                }

                Instantiate(BulletSparkPrefab, hitPoint, Quaternion.identity);
            }

        }

        private void Start()
        {
            ShootGunInputReference.action.Enable();
            ShootGunInputReference.action.performed += OnShootButtonPressed;
        }

        private void OnDestroy()
        {
            ShootGunInputReference.action.Disable();
        }
    }
}