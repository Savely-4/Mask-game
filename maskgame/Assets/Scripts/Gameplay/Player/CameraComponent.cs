using UnityEngine;
using UnityEngine.InputSystem;

public class CameraComponent : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private float cameraSmoothnees = 1.25f;
    [SerializeField] private float cameraSensitivity = 2f;
    [SerializeField] private float maxSensitivity =1f;
    [SerializeField] private float rotationAmplitude = 5f;
    [SerializeField] private float recoverySmooth = 2f;
    [SerializeField] private float walkZSmooth = 0.1f;
    [SerializeField] private float bobbingSmooth = 1f;
    [SerializeField] private float runAmplitude = 0.1f;
    [SerializeField] private float walkAmplitude = 0.1f;
    [SerializeField] private float walkBobbingSpeed = 5f;
    [SerializeField] private float stayAmplitude = 0.1f;
    [SerializeField] private float stayBobbingSpeed = 2f;

    InputAction CameraInput;
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
    private float _newRotatinZEulerBobbing;
    private float _localMaxSensitivity;
    private float _mousePositionX;
    private float _mousePositionY;
    private float _eulerZ;
    
    private bool _IsMoving;
    
    private void Start()
    {
        CameraInput = InputSystem.actions.FindAction("Look");
        _currentAmplitude = stayAmplitude;
        _currentBobbingSpeed = stayBobbingSpeed;
        _oldRotationZEulerBobbing = 0;
        _oldRotationZEuler = this.transform.eulerAngles.z;
    }
    
    private void Update()
    {
        Rotate();
        Bobbing();
        
    }
    
    private void Rotate()
    {
        CameraVector = CameraInput.ReadValue<Vector2>() * cameraSensitivity;
        
        _localMaxSensitivity = (((maxSensitivity * Time.deltaTime) * 2f) * 100f);
        _mousePositionX = Mathf.Clamp(CameraVector.x, -_localMaxSensitivity, _localMaxSensitivity);
        _mousePositionY = Mathf.Clamp(CameraVector.y, -_localMaxSensitivity, _localMaxSensitivity);
        _yRotation -= _mousePositionX;
        _xRotation -= _mousePositionY;
        _xRotation = Mathf.Clamp(CameraVector.x, -80f, 80f);
        _xRot = Mathf.Lerp(_xRot, _xRotation, cameraSmoothnees * Time.deltaTime);
        _yRot = Mathf.Lerp(_yRot, _yRotation, cameraSmoothnees * Time.deltaTime);
        _eulerZ = transform.eulerAngles.z;
        
        if (_eulerZ > 180) _eulerZ -= 360;
        
        _newRotatinZEulerBobbing = Mathf.Lerp(_newRotatinZEulerBobbing, _oldRotationZEulerBobbing, walkZSmooth * Time.deltaTime);
        _newRotationZ = Mathf.SmoothDamp(_eulerZ, _newRotatinZEulerBobbing, ref _velocityZ, recoverySmooth * Time.deltaTime);
        
        transform.rotation = Quaternion.Euler(_xRot,-_yRot,_newRotationZ);
    }

    private void Bobbing()
    {
        _currentBobbing = Mathf.Lerp(_currentBobbing, (_stayBobbing + _walkBobbing), Time.deltaTime);
        
        if(_IsMoving)
        {
            _timer += Time.deltaTime * _currentBobbingSpeed;
            walkBobbingSpeed = Mathf.Sin(_timer) * rotationAmplitude;
            _walkBobbing = Mathf.Lerp(_walkBobbing, _walkBobbing, Time.deltaTime * bobbingSmooth);
            _oldRotationZEulerBobbing = ((_oldRotationZEuler + walkBobbingSpeed) - CameraVector.x * 2f);
        }
        else
        {
            _timerStay += Time.deltaTime * stayBobbingSpeed;
            _stayBobbing = Mathf.Sin(_timerStay) * stayAmplitude;
            _stayBobbing = Mathf.Lerp(_stayBobbing, _stayBobbing, Time.deltaTime * bobbingSmooth);
            _oldRotationZEulerBobbing = _oldRotationZEuler;
        }
    }
}

