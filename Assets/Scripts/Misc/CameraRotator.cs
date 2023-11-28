using UnityEngine;

/// <summary>
///  ласс, отвечающий за случайное вращение камеры
/// </summary>
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
        RotateX = Random.Range(-1f, 1f) * rotationSpeed;
        RotateY = Random.Range(-1f, 1f) * rotationSpeed;
        RotateZ = Random.Range(-1f, 1f) * rotationSpeed;
    }

    void Update()
    {
        gameObject.transform.Rotate(RotateX * Time.deltaTime , RotateY * Time.deltaTime, RotateZ * Time.deltaTime);
    }
}
