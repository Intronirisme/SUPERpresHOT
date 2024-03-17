using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        //The player will be able to pick up the item again and change it's layer if needed
        if (gameObject.layer == LayerMask.NameToLayer("Projectile") && _rb.velocity.magnitude <= Mathf.Epsilon)
        {
            gameObject.layer = LayerMask.NameToLayer("Item");
            GameMaster.Instance.RemoveObject(this);
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

        StartCoroutine(FreezeCall());
    }

    public void Freeze()
    {
        _frozenVelocity = _rb.velocity; //If you freeze an iteam already frozen it will have a velocity of 0
        _rb.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer("Frozen");
    }

    public void Unfreeze()
    {
        Debug.Log("unfreeze");
        _rb.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Projectile");
        _rb.velocity = _frozenVelocity;
    }

    private void ResumeUse()
    {

    }

    private void ResumeThrow()
    {

    }

    private IEnumerator FreezeCall()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Frozen"))
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);

        Freeze();
    }
}
