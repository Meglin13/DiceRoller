﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public event Action OnDiceStop = delegate { };

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
#endif

    public IEnumerator WaitTillStop()
    {
        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() => rb.velocity == Vector3.zero);

        OnDiceStop();
    }

    public int GetUpperSide()
    {
        foreach (var item in Sides)
        {
            if (!Physics.Raycast(item.transform.position, item.transform.forward, rayLength, 1 << 6))
            {
                return int.Parse(item.name);
            }
        }

        return -1;
    }

    private void OnDrawGizmos()
    {
        foreach (var item in Sides)
        {
            Gizmos.DrawLine(item.transform.position, item.transform.position + (item.transform.forward * rayLength));
        }
    }
}