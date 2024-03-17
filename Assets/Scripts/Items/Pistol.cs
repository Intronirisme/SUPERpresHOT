using UnityEngine;
using UnityEngine.UIElements;

public class Pistol : Item
{
    public GameObject Bullet;

    private Transform _nuzzle;
    private Camera _cam;

    public override void Init()
    {
        PlayerCanThrow = true;
        PlayerCanUse = true;
        _nuzzle = transform.Find("Nuzzle");
        _cam = Camera.main;
        ProjectileType = ProjectileTypes.Hard;
    }

    public override void Use(int layer)
    {
        RaycastHit hit;

        if (Physics.Raycast(_cam.transform.position, _cam.transform.TransformDirection(Vector3.forward), out hit))
        {
            if (hit.collider != null)
            {
                Vector3 direction = (hit.point - _nuzzle.position).normalized;
                Vector3 velocity = direction * 100f;

                GameObject bullet = Instantiate(Bullet, _nuzzle.position, Quaternion.LookRotation(direction));

                Bullet bulletComponent = bullet.GetComponent<Bullet>();
                bulletComponent.SetBulletVelocity(velocity);

                StartCoroutine(bulletComponent.FreezeCall(layer, 0.01f));
            }
        }
    }
}
