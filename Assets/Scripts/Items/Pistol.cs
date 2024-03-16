using UnityEngine;

public class Pistol : MonoBehaviour, IInteractable
{
    public bool canBeUsed;

    public void Take()
    {
        Debug.Log(gameObject.name + " has been taken");
    }

    public void PutDown()
    {
        Debug.Log(gameObject.name + " has been put down");
    }

    public void Throw()
    {
        Debug.Log(gameObject.name + " has been throwned");

        Stop();
    }

    public void Use()
    {
        if (canBeUsed)
        {
            Debug.Log(gameObject.name + " has been used");
        }
    }

    public void Play()
    {
        Debug.Log(gameObject.name + " has been unfreezed");
    }

    public void Stop()
    {
        Debug.Log(gameObject.name + " has been freezed");
    }
}
