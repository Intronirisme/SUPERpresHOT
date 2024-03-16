using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileTypes
{
    Null,
    Hard,
    Soft,
    Cut,
    Burn
}

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    [Header("Interactions")]
    public ProjectileTypes ProjectileType = ProjectileTypes.Null;
    public float SnapDuration = .6f;

    protected bool PlayerCanUse = false;
    public bool CanUse { get { return PlayerCanUse; } }
    protected bool PlayerCanThrow = false;
    public bool CanThrow { get { return PlayerCanThrow; } }

    private bool _isHeld = false;

    private float _remainingSnap;

    void Update()
    {
        if (_isHeld && _remainingSnap > 0)
        {
            _remainingSnap -= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.parent.position, transform.position, _remainingSnap/SnapDuration);
            transform.rotation = Quaternion.Slerp(transform.parent.rotation, transform.rotation, _remainingSnap/SnapDuration);
        }
    }

    public void Pickup(GameObject AttachPoint)
    {
        transform.parent = AttachPoint.transform;
        _isHeld = true;
        _remainingSnap = SnapDuration;
    }

    public void Drop()
    {
        _isHeld = false;
        transform.parent = null;
        _remainingSnap = 0;
    }

    public void Use()
    {

    }

    public void Throw()
    {

    }

    public void Freeze()
    {

    }

    public void Unfreeze()
    {
         
    }

    private void ResumeUse()
    {

    }

    private void ResumeThrow()
    {

    }
}
