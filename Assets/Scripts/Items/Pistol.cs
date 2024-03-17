using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Pistol : Item
{
    public GameObject Bullet;

    private Transform _nuzzle;
    private Camera _cam;

    private float _recoilForce = 30f;

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

                ShootBullet(direction, layer);

                Recoil(hit, layer);
            }
        }
        else
        {
            Vector3 endPos = (_cam.transform.TransformDirection(Vector3.forward)) * 1000f;
            Vector3 direction = (endPos - _nuzzle.position).normalized;

            ShootBullet(direction, layer);

            Vector3 velocity = _recoilForce * -direction / _rb.mass;
            gameObject.layer = LayerMask.NameToLayer("Frozen");

            Throw(velocity, layer);
        }
    }

    public void Recoil(RaycastHit hit, int layer)
    {
        Vector3 direction = (hit.point - _nuzzle.position).normalized;
        Vector3 velocity = _recoilForce * -direction / _rb.mass;
        gameObject.layer = LayerMask.NameToLayer("Frozen");

        Throw(velocity, layer);
    }

    public void ShootBullet(Vector3 direction, int layer)
    {
        Vector3 velocity = direction * 100f;

        GameObject bullet = Instantiate(Bullet, _nuzzle.position, Quaternion.LookRotation(direction));

        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.SetBulletVelocity(velocity);

        StartCoroutine(bulletComponent.FreezeCall(layer, 0.01f));
    }
}
