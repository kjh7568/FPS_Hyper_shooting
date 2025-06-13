using System;
using Unity.VisualScripting;
using UnityEngine;

public class Grenade : Gun
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        rb.velocity += Camera.main.transform.forward * 20f;
    }
    
    public override void Fire()
    {
        throw new System.NotImplementedException();
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }
}