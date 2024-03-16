using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Vector3 _frozenVelocity = Vector3.zero;

    private Rigidbody _rb;

    private void Awake()
    {
        Debug.Log("Awake");
        _rb = GetComponent<Rigidbody>();

        Init();
    }

    public virtual void Init() { }

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

        _rb.isKinematic = true;

        gameObject.layer = LayerMask.NameToLayer("Frozen");
    }

    public void Drop()
    {
        _isHeld = false;
        transform.parent = null;
        _remainingSnap = 0;

        _rb.isKinematic = false;

        gameObject.layer = LayerMask.NameToLayer("Item");
    }

    public void Use()
    {

    }

    public void Throw(Vector3 velocity)
    {
        _isHeld = false;
        transform.parent = null;
        _remainingSnap = 0;

        _rb.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Item");

        _rb.velocity = velocity;
    }

    public void Freeze()
    {
        _rb.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer("Frozen");
    }

    public void Unfreeze()
    {
        _rb.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Projectile");
    }

    private void ResumeUse()
    {

    }

    private void ResumeThrow()
    {

    }

    private IEnumerator FreezeCall()
    {
        yield return new WaitForSeconds(0.5f);

        _frozenVelocity = _rb.velocity;
        Freeze();
    }
}
