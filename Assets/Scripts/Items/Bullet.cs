using UnityEngine;

public class Bullet : Item
{
    float _aliveTime = 0f;
    float _maxAliveTime = 5f;

    public override void Init()
    {
        ProjectileType = ProjectileTypes.Cut;
    }

    public void SetBulletVelocity(Vector3 velocity)
    {
        _rb.velocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Destroy(gameObject);
            GameMaster.Instance.RemoveObject(this);
        }
    }

    public override void ItemLifeTime()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            _aliveTime += Time.deltaTime;

            if (_aliveTime > _maxAliveTime)
            {
                Destroy(gameObject);
                GameMaster.Instance.RemoveObject(this);
            }
            Debug.Log(_aliveTime);
        }
    }
}
