using UnityEngine;

public class PlayerItemHolder
{
    private readonly Transform _holderPoint;

    private Transform _transformParent;
    private Transform _transform;
    
    public PlayerItemHolder(Transform holderPoint)
    {
        _holderPoint = holderPoint;
    }

    public void PickupView(Transform transform)
    {
        _transform = transform;

        if (_transform.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        _transformParent = _transform.parent;
        _transform.SetParent(_holderPoint);
        _transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
    
    public void DropView() 
    {
        if(_transform.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.AddForce(_holderPoint.forward * 5f, ForceMode.Impulse);
        }

        _transform.transform.SetParent(_transformParent);
        _transform = null;
        _transformParent = null;
    }
}
