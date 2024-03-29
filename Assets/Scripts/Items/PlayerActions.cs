using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    private float _pickUpRange = 10f;
    public float _throwForce = 20f;

    private int _currentLayer = 0;

    //public float ThrowAngle = 20f;

    private Item _itemInHand = null;

    private LayerMask _itemLayer;
    private LayerMask _frozenLayer;
    private bool _isAiming;

    private LineRenderer _lineRenderer;
    private Transform _hand;
    private Transform _camRoot;
    private Item _focusedItem; //TODO change this to Item type

    [Header("Display line")]
    [SerializeField]
    private int _linePoint = 25;
    [Range(0.01f, 0.25f)]
    private float _timeBetweenPoints = 0.1f;

    public Image _crossHair;

    private void Awake()
    {
        _hand = Helpers.FindInChildren(transform, "Hand");
        _itemLayer = LayerMask.GetMask("Item");
        _frozenLayer = LayerMask.GetMask("Frozen");

        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _lineRenderer.enabled = false;
        _camRoot = transform.Find("CameraRoot");

        _crossHair.color = Color.red;
    }

    private void Update()
    {
        RaycastHit hit;
        //Should we also add the projectile layer ?
        if (Physics.Raycast(_camRoot.position, _camRoot.TransformDirection(Vector3.forward), out hit, _pickUpRange, _itemLayer | _frozenLayer)) //we check if there is an item in the ray
        {
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out Item item))
                {
                    _focusedItem = item;
                }
                else
                {
                    _focusedItem = null;
                }
            }
            else
            {
                _focusedItem = null;
                Debug.Log("item unfocused");
            }
        }
        else
        {
            _focusedItem = null;
        }

        Aim();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            TakeItem();
        }
    }

    public void Use(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UseItem();
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (context.started && _itemInHand != null)
        {
            _isAiming = true;
            _lineRenderer.enabled = true;
        }
        else if (context.canceled)
        {
            _isAiming = false;
            _lineRenderer.enabled = false;
            ThrowItem();
        }
    }

    public void TimeSwitch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _currentLayer = ++_currentLayer > 2 ? 0 : _currentLayer;

            switch(_currentLayer)
            {
                case 0: _crossHair.color = Color.red; break;
                case 1: _crossHair.color = Color.green; break;
                case 2: _crossHair.color = Color.blue; break;
                default: _crossHair.color = Color.white; break;
            }

            Debug.Log(_currentLayer);
        }
    }

    private void TakeItem()
    {
        if (_focusedItem != null)
        {
            if (_itemInHand != null) //if we already have an item we first put down the item in hand
            {
                _itemInHand.Drop();
                _itemInHand = null;
            }

            _focusedItem.Pickup(_hand.gameObject);
            _itemInHand = _focusedItem;
            GameMaster.Instance.RemoveObject(_itemInHand);
        }
        else
        {
            if (_itemInHand != null)
            {
                _itemInHand.Drop();
                _itemInHand = null;
            }
        }
    }

    private void UseItem()
    {
        if (_itemInHand != null)
        {
            _itemInHand.Use(_currentLayer);
        }
    }

    private void ThrowItem()
    {
        if (_itemInHand != null)
        {
            Vector3 startVelocity = _throwForce * _camRoot.TransformDirection(Vector3.forward) / _itemInHand.GetComponent<Rigidbody>().mass;

            _itemInHand.Throw(startVelocity, _currentLayer);

            _itemInHand = null;
        }
    }

    public void Aim()
    {
        if (_isAiming)
        {
            Vector3 startPos = _hand.position + _camRoot.TransformDirection(Vector3.forward) * 0.5f;

            float mass = _itemInHand.GetComponent<Rigidbody>().mass;

            _lineRenderer.positionCount = Mathf.CeilToInt(_linePoint / _timeBetweenPoints) + 1;

            Vector3 startVelocity = _throwForce * _camRoot.TransformDirection(Vector3.forward) / mass;

            int i = 0;
            _lineRenderer.SetPosition(i, startPos);

            for (float time = 0; time < _linePoint; time += _timeBetweenPoints)
            {
                i++;
                Vector3 point = startPos + time * startVelocity;
                point.y = startPos.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

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

    public void ToggleItemFreeze(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameMaster.Instance.ToogleLayer(_currentLayer);
        }
    }
}
