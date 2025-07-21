using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Parker Albright
 * This script will contain all of the physics math for the simulation.
 */

public class GravitySimulator : MonoBehaviour
{
  [Header("Orbital Settings")]
  public float gravitationalConstant = 0.1f;
  [SerializeField] private CelestialBody[] allBodies;

  private void FixedUpdate()
  {
    allBodies = FindObjectsOfType<CelestialBody>();

    foreach (var bodyA in allBodies)
    {
      Vector2 netForce = Vector2.zero;

      foreach (var bodyB in allBodies)
      {
        if (bodyA == bodyB)
          continue;

        Vector2 direction = (Vector2)bodyB.transform.position - 
                            (Vector2)bodyA.transform.position;
        float distance = direction.magnitude + gravitationalConstant;
        Vector2 force = direction.normalized * gravitationalConstant
                      * bodyA.mass * bodyB.mass / (distance * distance);

        netForce = force;
      }
      Vector2 acceleration = netForce / bodyA.mass;
      bodyA.UpdateBody(acceleration, Time.fixedDeltaTime);
    }

    /*
    foreach (var body in allBodies)
      body.UpdateBody(Time.fixedDeltaTime);
    */
    }
}
