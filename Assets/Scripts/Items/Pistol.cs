using UnityEngine;

public class Pistol : MonoBehaviour, IInteractable
{
    public bool canBeUsed;

    public void Take()
    {
        Debug.Log(name + " has been taken");
    }

    public void PutDown()
    {
        Debug.Log(name + " has been put down");
    }

    public void Throw()
    {
        Debug.Log(name + " has been throwne");

        Stop();
    }

    public void Use()
    {
        if (canBeUsed)
        {
            Debug.Log(name + " has been used");
        }
        else
        {
            Debug.Log(name + " can't be used");
        }
    }

    public void Play()
    {
        Debug.Log(name + " has been unfreezed");
    }

    public void Stop()
    {
        Debug.Log(name + " has been freezed");
    }

    public GameObject GetItem()
    {
        return gameObject;
    }
}
