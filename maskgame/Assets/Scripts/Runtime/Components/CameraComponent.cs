using Runtime.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraComponent
{
    InputAction CameraInput;
    private CameraConfig _cameraConfig;
    
    public Vector3 CameraVector;
    public Vector3 Pos;

    private float _xRotation;
    private float _yRotation;
    private float _xRot;
    private float _yRot;
    private float _timer;
    private float _timerStay;
    private float _currentBobbingSpeed;
    private float _currentAmplitude;
    private float _walkBobbing;
    private float _stayBobbing;
    private float _currentBobbing;
    private float _oldRotationZEulerBobbing;
    private float _oldRotationZEuler;
    private float _newRotationZEuler;
    private float _newRotationZ;
    private float _velocityZ;
    private float _walkBobbingRotation;
    private float _newRotationZEulerBobbing;
    private float _localMaxSensitivity;
    private float _mousePositionX;
    private float _mousePositionY;
    private float _eulerZ;
    
    private bool _IsMoving;
    private float _newRotationZEulerSlant;
    private float angleSlantZ;
    private float _currentSlantAngle;

    public CameraComponent(CameraConfig config)
    {
        _cameraConfig = config;    
        CameraInput = InputSystem.actions.FindAction("Look");
        _currentAmplitude = _cameraConfig.StayAmplitude;
        _currentBobbingSpeed = _cameraConfig.StayBobbingSpeed;
        _oldRotationZEulerBobbing = 0;
    }

    public void SetOldRotationEulerZ(Transform cameraTransform)
    {
        _oldRotationZEuler = cameraTransform.eulerAngles.z;
    }
    
    public void UpdateCameraPosition(Transform cameraTransform, Vector3 originPos)
    {
        cameraTransform.transform.position = originPos + (Vector3.up * _currentBobbing);
    }
    
    public Quaternion Rotate(Transform cameraTransform, float currentSlantAngle)
    {
        CameraVector = CameraInput.ReadValue<Vector2>() * _cameraConfig.CameraSensitivity;
        
        _localMaxSensitivity = (((_cameraConfig.MaxSensitivity * Time.deltaTime) * 2f) * 100f);
        _mousePositionX = Mathf.Clamp(CameraVector.x, -_localMaxSensitivity, _localMaxSensitivity);
        _mousePositionY = Mathf.Clamp(CameraVector.y, -_localMaxSensitivity, _localMaxSensitivity);
        _yRotation -= _mousePositionX;
        _xRotation -= _mousePositionY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);
        _xRot = Mathf.Lerp(_xRot, _xRotation, _cameraConfig.CameraSmoothnees * Time.deltaTime);
        _yRot = Mathf.Lerp(_yRot, _yRotation, _cameraConfig.CameraSmoothnees * Time.deltaTime);
        
        _eulerZ = cameraTransform.eulerAngles.z;
        
        if (_eulerZ > 180) _eulerZ -= 360;
        
        angleSlantZ = _currentSlantAngle * 20f;
        angleSlantZ = Mathf.Clamp(angleSlantZ,-_cameraConfig.MaxZAmplitude, _cameraConfig.MaxZAmplitude);
        _newRotationZ = Mathf.SmoothDamp(_eulerZ, angleSlantZ, ref _velocityZ, _cameraConfig.RecoverySmooth * Time.deltaTime);
        cameraTransform.rotation = Quaternion.Euler(_xRot,-_yRot,_newRotationZ);
        return cameraTransform.rotation;
    }
    
    public void Bobbing(Vector2 currentPlayerMagnitude)
    {
        _currentBobbing = Mathf.Lerp(_currentBobbing, (_stayBobbing + _walkBobbing), Time.deltaTime);
        
        _IsMoving = currentPlayerMagnitude.magnitude > 0.0f;
        Debug.Log(currentPlayerMagnitude);
        
        if(_IsMoving)
        {
            _timer += Time.deltaTime * _currentBobbingSpeed;
            _walkBobbing = Mathf.Sin(_timer * _cameraConfig.WalkBobbingSpeed) * _cameraConfig.WalkAmplitude;
            _walkBobbing = Mathf.Lerp(_walkBobbing, _walkBobbing, Time.deltaTime * _cameraConfig.BobbingSmooth);
            _currentSlantAngle = -currentPlayerMagnitude.y * _cameraConfig.SlantZAmplitude;
        }
        else
        {
            _timerStay += Time.deltaTime * _cameraConfig.StayBobbingSpeed;
            _stayBobbing = Mathf.Sin(_timerStay) * _cameraConfig.StayAmplitude;
            _stayBobbing = Mathf.Lerp(_stayBobbing, _stayBobbing, Time.deltaTime * _cameraConfig.BobbingSmooth);
            _currentSlantAngle = 0f;
        }
    }
}

