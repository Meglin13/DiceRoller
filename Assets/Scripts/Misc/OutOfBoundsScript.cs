using UnityEngine;

public class OutOfBoundsScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = new Vector3(0, 5, 0);
    }
}
