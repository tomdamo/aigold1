using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject
{
    public NavMeshAgent Agent;
    public bool currentlySelected; 
    //after clicking on the enemy,
    //you want to be able to click on a point to where only he will walk to
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
