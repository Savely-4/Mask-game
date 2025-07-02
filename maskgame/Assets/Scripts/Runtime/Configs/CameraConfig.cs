using System;
using UnityEngine;

namespace Runtime.Configs
{
    [Serializable]
    public class CameraConfig
    {
        [field: SerializeField] public float CameraOffsetY = 0.5f;
        [field: SerializeField] public float CameraSmoothnees = 1.25f;
        [field: SerializeField] public float CameraSensitivity = 2f;
        [field: SerializeField] public float MaxSensitivity =1f;
        [field: SerializeField] public float RecoverySmooth = 2f;
        [field: SerializeField] public float BobbingSmooth = 1f;
        [field: SerializeField] public float SlantZAmplitude = 0.1f;
        [field: SerializeField] public float MaxZAmplitude;
        [field: SerializeField] public float WalkAmplitude = 0.1f;
        [field: SerializeField] public float WalkBobbingSpeed = 5f;
        [field: SerializeField] public float StayAmplitude = 0.1f;
        [field: SerializeField] public float StayBobbingSpeed = 2f;
    }
}