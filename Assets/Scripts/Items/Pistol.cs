using UnityEngine;

public class Pistol : Item
{
    public override void Init()
    {
        PlayerCanThrow = true;
        PlayerCanUse = true;
    }
}
