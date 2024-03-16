using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    private float _pickUpRange = 10f;
    public float _throwForce = 20f;

    public float ThrowAngle = 20f;

    private IInteractable _itemInHand = null;
    private List<IInteractable> _itemsInLayer = new List<IInteractable>(); //add a list for each timer layer ?

    private LayerMask _itemLayer;
    private bool _isAiming;

    private LineRenderer _lineRenderer;
    private Transform _hand;

    [Header("Display line")]
    [SerializeField]
    private int _linePoint = 25;

    [Range(0.01f, 0.25f)]
    private float _timeBetweenPoints = 0.1f;

    private void Awake()
    {
        _hand = transform.Find("Hand");
        _itemLayer = LayerMask.GetMask("Item");

        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _lineRenderer.enabled = false;

        ThrowAngle = ThrowAngle * Mathf.Deg2Rad;
    }

    private void Update()
    {
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
            Debug.Log("Aiming with item");
            _isAiming = true;
            _lineRenderer.enabled = true;
        }
        else if (context.canceled)
        {
            //_isAiming = false;
            //_lineRenderer.enabled = false;
            //ThrowItem();
        }
    }

    public void TimeSwitch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ItemTimeSwitch();
        }
    }

    private void TakeItem()
    {
        Camera cam = GetComponentInChildren<Camera>();
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, _pickUpRange, _itemLayer)) //we check if there is an item in the ray
        {
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out IInteractable intertactable))
                {
                    if (_itemInHand != null) //if we already have an item we first put down the item in hand
                    {
                        _itemInHand.PutDown();
                        _itemInHand = null;
                    }

                    intertactable.Take();
                    _itemInHand = intertactable;
                }
            }
        }
        else //if we don't touch anything we put down the item
        {
            if (_itemInHand != null)
            {
                _itemInHand.PutDown();
                _itemInHand = null;
            }
        }
    }

    private void UseItem()
    {
        if (_itemInHand != null)
        {
            _itemInHand.Use();
        }
        else
        {
            Debug.Log("No item in hand");
        }
    }

    private void ThrowItem()
    {
        if (_itemInHand != null)
        {
            _itemInHand.Throw();
            _itemsInLayer.Add(_itemInHand); //watch which time layer the hand is so we know in which list we put the item

            _itemInHand = null;
        }
        else
        {
            Debug.Log("No item in hand to be thrown");
        }
    }

    private void ItemTimeSwitch() //time layer as an argument
    {
        if (_itemsInLayer.Count > 0)
        {
            foreach (var item in _itemsInLayer)
            {
                item.Play();
            }

            _itemsInLayer.Clear();
        }
    }

    public void Aim()
    {
        if (_isAiming)
        {
            Camera cam = GetComponentInChildren<Camera>();

            Vector3 startPos = _hand.position + cam.transform.TransformDirection(Vector3.forward) * 0.5f; //change to hand position
            Vector3 endPos = cam.transform.TransformDirection(Vector3.forward) * 10f;
            Vector3 direction = endPos - startPos;

            float mass = _itemInHand.GetItem().GetComponent<Rigidbody>().mass;

            _lineRenderer.positionCount = Mathf.CeilToInt(_linePoint / _timeBetweenPoints) + 1;

            Vector3 startVelocity = _throwForce * cam.transform.TransformDirection(Vector3.forward) / mass;

            int i = 0;
            _lineRenderer.SetPosition(i, startPos);

            for (float time = 0; time < _linePoint; time += _timeBetweenPoints)
            {
                i++;
                Vector3 point = startPos + time * startVelocity;
                point.y = startPos.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

                _lineRenderer.SetPosition(i, point);
            }
        }
    }
}
