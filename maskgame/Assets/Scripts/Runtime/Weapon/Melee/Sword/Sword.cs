using System.Collections;
using System.Collections.Generic;
using Runtime.InventorySystem;
using UnityEngine;

namespace WeaponSystem.Melee.Types
{
    public class Sword : WeaponMelee, IAlternateAttackable, IPickableItem
    {
        private Coroutine _resetColorRoutine;

        public void AlternateAttack()
        {
            Debug.Log("Альтернативная атака");
        }

        protected override void OnHitTargets(Collision[] targets)
        {
            base.OnHitTargets(targets);

            List<(Material material, Color originalColor)> affectedMaterials = new();

            foreach (var target in targets)
            {
                if (target.collider.TryGetComponent<MeshRenderer>(out var renderer))
                {
                    var material = renderer.material;
                    affectedMaterials.Add((material, material.color));
                    material.color = Color.red;
                }
            }

            if (_resetColorRoutine == null)
                _resetColorRoutine = StartCoroutine(ResetColorRoutine(affectedMaterials));
        }
        private IEnumerator ResetColorRoutine(List<(Material material, Color originalColor)> affectedMaterials)
        {
            yield return new WaitForSeconds(0.3f);

            foreach (var (material, originalColor) in affectedMaterials)
            {
                material.color = originalColor;
            }

            _resetColorRoutine = null;
        }
    }
}
