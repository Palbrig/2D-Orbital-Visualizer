using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlanetSpawner : MonoBehaviour
{
  // CONSTANTS
  private Vector3 ORIGIN = new Vector3(0f, 0f, -10f);

  [Header("Spawn Controls")]
  public float spawnTimer = .1f; //sets timer for respawn
  private float timer; //used to calculate
  public GameObject planetPrefab;
  private Color spawnColor = Color.white;

  private Vector3 clickStartWorld;
  private bool isDragging = false;
  public float velocityScale = 1f;

  [Header("CamerControls")]
  public float zoomSpeed = 5f;
  public float minZoom = 2f;
  public float maxZoom = 20f;
  public float panSpeed = 1f;
  public Camera camera;

  [Header("Spawn Menu")]
  public Canvas canvas;
  private bool inMenu = false;
  public SpawnSettingsScript spawnSettings;

  public LineRenderer lineIndicator;

  private Vector3 dragOrigin;

  // Start is called before the first frame update
  void Start()
    {
    timer = spawnTimer;
    lineIndicator.enabled = false;
    canvas.enabled = false;
    spawnSettings.GetComponent<SpawnSettingsScript>().enabled = true;
  }

    // Update is called once per frame
  void Update()
  {
    if(!inMenu)
    {
      HandleSpawning();
      HandleZoom();
      HandlePan();
    }
    HandleMenu();
  }

  private void HandleSpawning()
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
      newBody.GetComponent<CelestialBody>().mass = spawnSettings.GetMass();
      newBody.GetComponent<CelestialBody>().initialVelocity = velocity;

      timer = spawnTimer;

      lineIndicator.enabled = false;
    }

    if (timer > 0f)
    {
      timer -= Time.deltaTime;
    }
  }

  private void HandleZoom()
  {
    float scroll = Input.GetAxis("Mouse ScrollWheel");
    if (scroll != 0f)
    {
      camera.orthographicSize -= scroll * zoomSpeed;
      camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);
    }
  }

  private void HandlePan()
  {
    if (Input.GetMouseButtonDown(2))
    {
      dragOrigin = camera.ScreenToWorldPoint(Input.mousePosition);
      dragOrigin.z = 0f;
    }

    if (Input.GetMouseButton(2))
    {
      Vector3 curPos = camera.ScreenToWorldPoint(Input.mousePosition);
      curPos.z = 0f;
      Vector3 difference = dragOrigin - curPos;
      camera.transform.position += difference;
    }

    if (Input.GetKey("space"))
    {
      camera.transform.position = ORIGIN;
    }
  }

  private void HandleMenu()
  {
    if (Input.GetKeyDown("escape"))
    {
      inMenu = !inMenu;
      canvas.enabled = inMenu;
    }
  }
}
