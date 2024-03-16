using UnityEngine;

public class TestController : MonoBehaviour
{
    public float RotationSpeed = 100f;
    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X"; 
    const string yAxis = "Mouse Y";

    // Update is called once per frame
    void Update()
    {
        Rotation();
    }

    private void Rotation()
    {
        rotation.x += Input.GetAxis(xAxis) * RotationSpeed * Time.deltaTime;
        rotation.y += Input.GetAxis(yAxis) * RotationSpeed * Time.deltaTime;

        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = xQuat * yQuat;
    }
}
