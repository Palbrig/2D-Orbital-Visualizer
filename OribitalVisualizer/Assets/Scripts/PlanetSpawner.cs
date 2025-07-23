using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
  public float spawnTimer = 1f; //sets timer for respawn
  private float timer; //used to calculate
  public GameObject planetPrefab;
  private float spawnMass = 1f;
  private Color spawnColor = Color.white;

  private Vector3 clickStartWorld;
  private bool isDragging = false;
  public float velocityScale = 1f;

  public LineRenderer lineIndicator;

    // Start is called before the first frame update
    void Start()
    {
    timer = spawnTimer;
    lineIndicator.enabled = false;
    }

    // Update is called once per frame
  void Update()
  {
      if (Input.GetMouseButtonDown(0) && timer <= 0f)
      {
        clickStartWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickStartWorld.z = 0f;
        isDragging = true;
        
        // Activate drag line
        lineIndicator.enabled = true;
        lineIndicator.SetPosition(0, clickStartWorld);
      }

      if (Input.GetMouseButton(0) && isDragging)
      {
      Vector3 currentMouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      currentMouseWorld.z = 0f;

      lineIndicator.SetPosition(0, clickStartWorld);
      lineIndicator.SetPosition(1, currentMouseWorld);
      }

      if (Input.GetMouseButtonUp(0) && isDragging) 
      {
        Vector3 clickEndWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickEndWorld.z = 0f;
        isDragging = false;

        Vector3 spawnPos = clickStartWorld;
        Vector2 velocity = (clickStartWorld - clickEndWorld) * velocityScale;


        GameObject newBody = Instantiate(planetPrefab, spawnPos, Quaternion.identity);
        newBody.GetComponent<CelestialBody>().mass = spawnMass;
        newBody.GetComponent<CelestialBody>().initialVelocity = velocity;

      timer = spawnTimer;

      lineIndicator.enabled = false;
      }

      if (timer >0f)
      {
        timer -= Time.deltaTime;
      }
  }
}
