using System;
using UnityEngine;

[Serializable]
public struct Orientation 
{
    public Vector3 Position;
    public Quaternion Rotation;
}

[Serializable]
public struct LocalOrientationObjects 
{
    [Header("Key")][SerializeField] public Type ObjectType;
    [Header("Value")][SerializeField] public Orientation Orientation;
}
public class PlayerItemHolder
{
    private readonly PlayerItemHolderConfig _config;
    private readonly Transform _holderPoint;

    //private Transform _transformParent;
    private Transform _transform;
    
    public PlayerItemHolder(PlayerItemHolderConfig config, Transform holderPoint)
    {
        _config = config;
        _holderPoint = holderPoint;
    }

    public void PickupView(Transform transform)
    {
        _transform = transform;

        if (_transform.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        //_transformParent = _transform.parent;

        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        
        if (_config.LocalOrientationObjects.TryGetValue(transform.GetType(), out var orientation)) 
        {
            pos = orientation.Position;
            rot = orientation.Rotation;
        }

        _transform.SetParent(_holderPoint);
        _transform.SetLocalPositionAndRotation(pos, rot);
    }
    
    public void DropView() 
    {
        if(_transform.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.AddForce(_holderPoint.forward * 5f, ForceMode.Impulse);
        }

        _transform.transform.SetParent(null);
        _transform = null;
        //_transformParent = null;
    }
}
