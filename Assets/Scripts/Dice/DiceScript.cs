using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за логику кубика
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class DiceScript : MonoBehaviour
{
    public Rigidbody rb;

    [SerializeField]
    private Transform sidesContainer;

    [SerializeField]
    private List<Transform> Sides;

    [SerializeField]
    private float rayLength = 5;

    [SerializeField]
    [Range(0.6f, 1f)]
    private float range = 1;

    /// <summary>
    /// Событие, которое вызывается при остановке кубика
    /// </summary>
    public event Action OnDiceStop = delegate { };

    [SerializeField]
    private List<Transform> sides;

#if UNITY_EDITOR
    private void OnValidate()
    {
        var list = new List<Transform>();

        foreach (Transform item in sidesContainer)
        {
            list.Add(item);

            Vector3 direction = item.transform.position - transform.position;

            item.transform.rotation = Quaternion.LookRotation(direction);

            direction = direction.normalized * range;

            item.position = transform.position + direction;
        }

        Sides = list.OrderBy(x => int.Parse(x.name)).ToList();

        rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        foreach (var item in Sides)
        {
            Gizmos.DrawLine(item.transform.position - item.transform.forward, item.transform.position + (item.transform.forward * rayLength));
        }
    }
#endif

    private void OnDestroy() => OnDiceStop = null;

    /// <summary>
    /// Метод, отвечающий за вызов события остановки кубика. Вызывается как корутин
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitTillStop()
    {
        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() => rb.velocity == Vector3.zero);

        OnDiceStop();
    }

    /// <summary>
    /// Метод определяющий сторону кубика
    /// </summary>
    /// <returns>Число на верхней стороне кубика</returns>
    public int GetUpperSide()
    {
        int result = -1;
        var longestDistance = 0f;

        // Перебор списка сторон
        foreach (var item in Sides)
        {
            // Из стороны выпускается лучь перпендикулярно стороне кубика
            
            if (Physics.Raycast(item.transform.position - item.transform.forward, item.transform.forward, out RaycastHit ray, rayLength, 1 << 6))
            {
                // Если луч задел стены, окружающие кубик, то в качестве верхней стороны регистрируется сторона с длиннейшим лучом.
                // Это нужно для тех случаев, когда кубик стоит на грани.
                if (ray.distance > longestDistance)
                {
                    longestDistance = ray.distance;
                    result = int.Parse(item.name);
                }
            }
            else
            {
                // Если луч не попал в одну из стен, то сторона, из которой он был выпущен, является верхней
                result = int.Parse(item.name);
                return result;
            }
        }

        return result;
    }
}
