using System.Collections;
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
[RequireComponent(typeof(LineRenderer))]
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
    private bool _isAiming = false;

    private float _remainingSnap;
    private Vector3 _frozenVelocity = Vector3.zero;

    protected Rigidbody _rb;
    private LineRenderer _lineRenderer;
    private Color _lineColor;

    [Header("Display line")]
    [SerializeField]
    private int _linePoint = 25;
    [Range(0.01f, 0.25f)]
    private float _timeBetweenPoints = 0.1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.enabled = false;

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

        Aim();

        ItemLifeTime(); //to destroy projectile if they're in the void
    }

    public void Pickup(GameObject AttachPoint)
    {
        transform.parent = AttachPoint.transform;
        _isHeld = true;
        _remainingSnap = SnapDuration;

        _rb.isKinematic = true;

        gameObject.layer = LayerMask.NameToLayer("Frozen");

        if (_isAiming)
        {
            _isAiming = false;
            _lineRenderer.enabled = false;
        }
    }

    public void Drop()
    {
        _isHeld = false;
        transform.parent = null;
        _remainingSnap = 0;

        _rb.isKinematic = false;

        gameObject.layer = LayerMask.NameToLayer("Item");
    }

    public virtual void Use(int layer) { }

    public void Throw(Vector3 velocity, int layer)
    {
        _isHeld = false;
        transform.parent = null;
        _remainingSnap = 0;

        _rb.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Frozen");

        _rb.velocity = velocity;

        StartCoroutine(FreezeCall(layer, 0.1f));
    }

    public void Freeze()
    {
        _frozenVelocity = _rb.velocity; //If you freeze an iteam already frozen it will have a velocity of 0
        _rb.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer("Frozen");

        _isAiming = true;
        _lineRenderer.enabled = true;
    }

    public void Unfreeze()
    {
        Debug.Log("unfreeze");
        _rb.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Projectile");
        _rb.velocity = _frozenVelocity;

        _isAiming = false;
        _lineRenderer.enabled = false;
    }

    public IEnumerator FreezeCall(int layer, float timer)
    {
        yield return new WaitForSeconds(timer);

        if (gameObject.layer == LayerMask.NameToLayer("Frozen"))
        {
            yield return null;
        }
        GameMaster.Instance.AddObject(this, layer);
        
        switch (layer)
        {
            case 0: _lineColor = new Color(1f, 0f, 0f, 0.7f); break;
            case 1: _lineColor = new Color(0f, 1f, 0f, 0.7f); break;
            case 2: _lineColor = new Color(0f, 0f, 1f, 0.7f); break;
            default: _lineColor = Color.white; break;
        }

        _lineRenderer.startColor = _lineColor;

        Freeze();
    }

    public void Aim()
    {
        if (_isAiming)
        {
            Vector3 startPos = transform.position + transform.TransformDirection(Vector3.forward) * 0.5f;

            _lineRenderer.positionCount = Mathf.CeilToInt(_linePoint / _timeBetweenPoints) + 1;

            Vector3 startVelocity = _frozenVelocity;

            int i = 0;
            _lineRenderer.SetPosition(i, startPos);

            for (float time = 0; time < _linePoint; time += _timeBetweenPoints)
            {
                i++;
                Vector3 point = startPos + time * startVelocity;

                if (_rb.useGravity)
                {
                    point.y = startPos.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
                }
                else
                {
                    point.y = startPos.y + startVelocity.y * time + (2f * time * time);
                }


                _lineRenderer.SetPosition(i, point);

                Vector3 lastpos = _lineRenderer.GetPosition(i - 1);

                RaycastHit hit;

                if (Physics.Linecast(lastpos, point, out hit))
                {
                    _lineRenderer.SetPosition(i, hit.point);
                    _lineRenderer.positionCount = i + 1;
                    return;
                }
            }
        }
    }

    public virtual void ItemLifeTime() { }
}
