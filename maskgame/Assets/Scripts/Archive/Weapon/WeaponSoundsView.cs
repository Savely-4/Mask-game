using System.Collections.Generic;
using Runtime.Entities.WeaponSystem;
using UnityEngine;

namespace Runtime.Services.WeaponSystem
{
    public class WeaponSoundsView : MonoBehaviour
    {
        [Header("Model")]
        [field: SerializeField] protected Weapon WeaponModel { get; private set; }


        [Header("Sound settings")]
        [field: SerializeField] protected List<AudioSource> AttackAudioSources { get; private set; }   

        protected virtual void OnEnable()
        {
            WeaponModel.OnAttack += AttackSoundPlay;
        }

        protected virtual void OnDisable()
        {
            WeaponModel.OnAttack -= AttackSoundPlay;
        }

        private void AttackSoundPlay() 
        {
            int count = AttackAudioSources.Count;
            int randIndex = Random.Range(0, count);
        
            AttackAudioSources[randIndex].Play();
        }
    }
}
