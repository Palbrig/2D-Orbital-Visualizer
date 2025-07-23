using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/*
 * Author: Parker Albright
 * This script will contain all the basics for celestial body math in this
 * physics simulation.
 */
public class CelestialBody : MonoBehaviour
{
  public enum BodyType { Asteroid, Planet, Star }

  [System.Serializable]
  public struct BodyMaterialMapping
  {
    public BodyType bodyType;
    public Material material;
  }

  // CONSTANTS
  // Mass cut offs for planets
  float SUN_MAX_MASS = 100f;
  float PLANET_MAX_MASS = 50f;
  float ASTEROIRD_MAX_MASS = 10f;

  [Header("Orbital Settings")]
   public Vector2 initialVelocity;
   public float mass = 1f;
   public Vector2 currentVelocity;

  [Header("Line Settings")]
  [SerializeField] private LineRenderer lineRenderer;
  [SerializeField] private List<Vector3> trailPositions = new List<Vector3>();
  [SerializeField] private int trailFadeCounter = 1;
  [SerializeField] private Color trailColor = Color.white;
  private Gradient gradient;

  [Header("Body Settings")]
  SpriteRenderer spriteRenderer;
  public float scaleFactor = 0.2f;
  public BodyType bodyType;

  [Header("Material Map")]
  public BodyMaterialMapping[] materialMappings;

  private Dictionary<BodyType, Material> materialDict;

  private void Start()
   {
    spriteRenderer = GetComponent<SpriteRenderer>();

    // Set Up Dictionary
    SetupMaterialDictionary();
    ClassifyAndApplyMaterial();
    ScaleByMass();

    currentVelocity = initialVelocity;

    lineRenderer = GetComponent<LineRenderer>();
    lineRenderer.positionCount = 0;
    SetGradient(); // Set's trail renderer settings
   }


  // Updates the celestial body
  // To be called from outside
  public void UpdateBody(Vector2 accelaration, float deltaTime)
  {
    UpdateVelocity(accelaration, deltaTime);
    UpdatePosition(deltaTime);
    UpdateTrail();
  }

  // Updates velocity each frame
  private void UpdateVelocity(Vector2 accelaration, float deltaTime)
  {
    currentVelocity += accelaration * deltaTime;
  }

  // Updates position each frame
  private void UpdatePosition(float deltaTime)
  {
    transform.position += (Vector3)(currentVelocity * deltaTime);
  }

  // Updates Trail
  private void UpdateTrail()
  {
    AddTrailPoint();
  }

  // Adds trail point to the trail that follows the planet.
  private void AddTrailPoint()
  {
    if (trailPositions.Count == 0 
       || Vector3.Distance(trailPositions[trailPositions.Count - 1]
       , transform.position) > 0.1f ) 
    { 
      trailPositions.Add(transform.position);
      lineRenderer.positionCount = trailPositions.Count;
      lineRenderer.SetPositions(trailPositions.ToArray());
    }

    while (trailPositions.Count > trailFadeCounter)
    {
      trailPositions.RemoveAt(0);
    }
  }
  // Sets a gradient to the trail to have it fade out overtime
  private void SetGradient()
  {
    gradient = new Gradient();
    gradient.SetKeys(
      new GradientColorKey[]
      {
        new GradientColorKey(trailColor, 1f),
        new GradientColorKey(trailColor, 1f)
      },
      new GradientAlphaKey[]
      {
        new GradientAlphaKey(1f, 1f),
        new GradientAlphaKey(0f, 0f)
      });
    lineRenderer.colorGradient = gradient;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
        if (collision.gameObject.CompareTag("Celestial"))
        {
          Destroy(collision.gameObject);
          Destroy(gameObject);
        }
    }

  // Initializes dictionary
  private void SetupMaterialDictionary()
  {
    materialDict = new Dictionary<BodyType, Material>();

    foreach (var mapping in materialMappings)
    {
      if(!materialDict.ContainsKey(mapping.bodyType))
      {
        materialDict.Add(mapping.bodyType, mapping.material);
      }
    }
  }

  // Classifies and applies material based off mass
  private void ClassifyAndApplyMaterial()
  {
    if(mass <= ASTEROIRD_MAX_MASS)
    {
      bodyType = BodyType.Asteroid;
      Debug.Log($"Setting celestial bodies body type to {bodyType} with mass = {mass}");
    }
    if (mass <= PLANET_MAX_MASS && mass > ASTEROIRD_MAX_MASS)
    {
      bodyType = BodyType.Planet;
      Debug.Log($"Setting celestial bodies body type to {bodyType} with mass = {mass}");
    }
    if (mass <= SUN_MAX_MASS && mass > PLANET_MAX_MASS)
    {
      bodyType = BodyType.Star;
      Debug.Log($"Setting celestial bodies body type to {bodyType} with mass = {mass}");
    }

    if(materialDict != null && materialDict.ContainsKey(bodyType))
    {
      spriteRenderer.material = materialDict[bodyType];
    }
    else
    {
      Debug.Log($"No material found for {{bodyType}}");
    }
  }

  // Scales size of planet by mass
  private void ScaleByMass()
  {
    float scale = Mathf.Sqrt(mass) * scaleFactor;
    transform.localScale = new Vector3(scale, scale, 1f);
  }
}
