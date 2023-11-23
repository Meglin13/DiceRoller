using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField]
    [Range(1, 10)]
    private float rotationSpeed;

    private float RotateX;
    private float RotateY;
    private float RotateZ;

    void OnEnable()
    {
        RotateX = Random.Range(-1f, 1f);
        RotateY = Random.Range(-1f, 1f);
        RotateZ = Random.Range(-1f, 1f);
    }

    void Update()
    {
        gameObject.transform.Rotate(RotateX * Time.deltaTime * rotationSpeed, RotateY * Time.deltaTime * rotationSpeed, RotateZ * Time.deltaTime * rotationSpeed);
    }
}
