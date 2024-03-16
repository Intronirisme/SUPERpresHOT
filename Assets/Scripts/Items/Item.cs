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

public class Item : MonoBehaviour
{
    [Header("Interactions")]
    public bool PlayerCanUse = false;
    public bool PlayerCanThrow = false;
    public ProjectileTypes ProjectileType = ProjectileTypes.Null;

    public bool CanUse { get { return PlayerCanUse; } }
    public bool CanThrow { get { return PlayerCanThrow; } }

    public void Pickup(GameObject AttachPoint)
    {

    }

    public void Drop()
    {

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
