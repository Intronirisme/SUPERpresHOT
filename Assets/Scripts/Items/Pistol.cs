using UnityEngine;

public class Pistol : Item
{
    private void Awake()
    {
        PlayerCanThrow = true;
        PlayerCanUse = true;
    }
}
