using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLIFramework.Scripts
{
    public class ExplodingBarrelsBehavior : MonoBehaviour
    {
        /// <summary>
        /// Gameobject that holds the explosion particle effect
        /// </summary>
        [field: SerializeField, Tooltip("Gameobject that holds the explosion particle effect"), Header("Object References")]
        public GameObject ExplosionGameObject { get; private set; } = null;
        /// <summary>
        /// Amount of health the barrel has before exploding
        /// </summary>
        [field: SerializeField, Tooltip("Amount of health the barrel has before exploding")]
        public float BarrelHealth { get; private set; } = 10f;
        /// <summary>
        /// Reference to the Mesh renderer of the barrel
        /// </summary>
        [field: SerializeField, Tooltip("Reference to the Mesh renderer of the barrel")]
        public MeshRenderer BarrelMeshRenderer { get; private set; } = null;

        /// <summary>
        /// Method to damage the barrel 
        /// </summary>
        /// <param name="damageAmount"></param>
        public void DamageBarrel(int damageAmount)
        {
            if (BarrelHealth > 0)
                BarrelHealth -= damageAmount;
                
            //Play the explosion if the barrel has 0 health and the explosion prefab is not already on
            if(BarrelHealth <= 0 && !ExplosionGameObject.activeSelf)
                StartCoroutine(DestroyingBarrel());
        }

        private IEnumerator DestroyingBarrel()
        {
            BarrelMeshRenderer.enabled = false;
            ExplosionGameObject.SetActive(true);
            yield return new WaitForSeconds(4f);
            Destroy(gameObject);
        }
    }
}

