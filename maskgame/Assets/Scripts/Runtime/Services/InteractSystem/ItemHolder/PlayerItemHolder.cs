using UnityEngine;

public class PlayerItemHolder
{
    private readonly Transform _holderPoint;

    private Rigidbody _body;
    
    public PlayerItemHolder(Transform holderPoint)
    {
        _holderPoint = holderPoint;
    }

    public void PickupView(Rigidbody body)
    {
        _body = body;

        if (_body != null) 
        {
            _body.isKinematic = true;
        }
        
        _body.transform.SetParent(_holderPoint);
        _body.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
    
    public void DropView() 
    {
        if (_body != null) 
        {
            _body.isKinematic = false;
            
            _body.AddForce(_holderPoint.forward * 5f, ForceMode.Impulse);
        }
    }
}
