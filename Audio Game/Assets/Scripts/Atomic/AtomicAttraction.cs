using UnityEngine;
using System.Collections.Generic;

public class AtomicAttraction : MonoBehaviour
{
	#region Variables
	[Tooltip("The audio playback GameObject. The audio compilation to visualize!")]
	public AudioPlayback audioPlayback;

	[Tooltip("The atom prefab")]
	public GameObject atomPrefab;
	[Tooltip("Attractor prefab.")]
	public GameObject attractorPrefab;
	[Tooltip("Gradient of colours for the atoms.")]
	public Gradient gradient;
	[Tooltip("The audio bands to create attractors from.")]
	public int[] attractPoints;
	[Tooltip("The direction the spacing/attractors will go.")]
	public Vector3 spacingDirection;
	[Tooltip("The space between the attractors.")]
	[Range(0, 20)]
	public float spacingBetweenAttractPoints;
	[Tooltip("The scale of the attractors.")]
	[Range(0, 10)]
	public float scaleAttractPoints;
	[Tooltip("The number of atoms for each attractor.")]
	[Range(0, 64)]
	public int numberOfAtoms;
	[Tooltip("Max random distance from the attractor's origin for the atom to spawn at.")]
	public float maxRandomAtomDistance;
	[Tooltip("The min and max scale of an atom. X is min, Y is max")]
	public Vector2 atomScaleMinMax;
	[Tooltip("Whether the atoms should use gravity")]
	public bool useGravity;

	[Header("Variables for setting up atoms")]
	public float strengthOfAttraction;
	public float maxMagnitude;

	[Header("Variables for explicit audio visualization things.")]
	public float audioScaleMultiplier;
	public float audioEmissionMultiplier;
	[Range(0.0f, 1.0f)]
	public float emissionThreshold;
	public Material material;


	private GameObject[] attractorList, atomList;
	private float[] atomScaleSet;

	private Material[] sharedMaterials;
	private Color[] sharedColors;

	public Color defaultColor;
	public Color correctInputColor;
	public Color incorrectInputColor;
	public float inputEffectLength;
	private bool correctInput, incorrectInput;
	private float correctEffectTime, incorrectEffectTime;

	#endregion

	#region Monobehaviour Methods
	void Start()
	{
		attractorList = new GameObject[attractPoints.Length];
		atomList = new GameObject[attractPoints.Length * numberOfAtoms];
		atomScaleSet = new float[attractPoints.Length * numberOfAtoms];
		sharedMaterials = new Material[attractPoints.Length * numberOfAtoms];
		sharedColors = new Color[attractPoints.Length * numberOfAtoms];

		// Instantiate all the attractors
		for (int i = 0; i < attractPoints.Length; ++i)
		{
			GameObject attractorInstance = Instantiate(attractorPrefab);
			attractorInstance.name = "Attractor: " + i;
			attractorList[i] = attractorInstance;

			attractorInstance.transform.position = new Vector3(transform.position.x + (spacingBetweenAttractPoints * i * spacingDirection.x),
															transform.position.y + (spacingBetweenAttractPoints * i * spacingDirection.y),
															transform.position.z + (spacingBetweenAttractPoints * i * spacingDirection.z));

			attractorInstance.transform.parent = this.transform;
			attractorInstance.transform.localScale = new Vector3(scaleAttractPoints, scaleAttractPoints, scaleAttractPoints);


			GameObject atomContainer = new GameObject {
				name = "Atom Containter: " + i
			};
			atomContainer.transform.parent = this.transform;

			// Set up the same material for all atoms of the same attractor
			Material attractorMaterial = new Material(material);
			sharedMaterials[i] = attractorMaterial;
			float evaluateStep = 1.0f / attractPoints.Length;
			sharedColors[i] = gradient.Evaluate(evaluateStep * i);

			// Instantiate all the atoms of the attractors
			GameObject atomInstance;

			for (int j = 0; j < numberOfAtoms; ++j)
			{
				int currentAtomCount = i * numberOfAtoms + j;
				atomInstance = Instantiate(atomPrefab);
				atomList[currentAtomCount] = atomInstance;

				atomInstance.transform.position = new Vector3(attractorInstance.transform.position.x + Random.Range(-maxRandomAtomDistance, maxRandomAtomDistance),
															attractorInstance.transform.position.y + Random.Range(-maxRandomAtomDistance, maxRandomAtomDistance),
															attractorInstance.transform.position.z + Random.Range(-maxRandomAtomDistance, maxRandomAtomDistance));

				atomInstance.GetComponent<AttractTo>().SetupAtom(attractorInstance.transform, maxMagnitude, strengthOfAttraction);

				float randomScale = Random.Range(atomScaleMinMax.x, atomScaleMinMax.y);
				atomScaleSet[currentAtomCount] = randomScale;
				atomInstance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

				if (useGravity)
				{
					atomInstance.GetComponent<Rigidbody>().useGravity = true;
				} else
				{
					atomInstance.GetComponent<Rigidbody>().useGravity = false;
				}

				atomInstance.transform.parent = atomContainer.transform;

				// Set atom material
				atomInstance.GetComponent<MeshRenderer>().material = sharedMaterials[i];
			}
		}
	}

	void Update()
	{
		if(audioPlayback.AudioIsPlaying()) AtomBehaviour();
	}
	#endregion

	#region Public Methods
	public void CorrectInputEffect()
	{
		correctEffectTime = Time.time;
		correctInput = true;
	}

	public void IncorrectInputEffect()
	{
		incorrectEffectTime = Time.time;
		incorrectInput = true;
	}
	#endregion

	#region Private/Protected Methods
	private void OnDrawGizmos()
	{
		for (int i = 0; i < attractPoints.Length; ++i)
		{
			// Gradient is from 0.0f to 1.0f
			float evaluateStep = 1.0f / attractPoints.Length;
			Color color = gradient.Evaluate(evaluateStep * i);
			Gizmos.color = color;

			Vector3 pos = new Vector3(transform.position.x + (spacingBetweenAttractPoints * i * spacingDirection.x),
								transform.position.y + (spacingBetweenAttractPoints * i * spacingDirection.y),
								transform.position.z + (spacingBetweenAttractPoints * i * spacingDirection.z));

			Gizmos.DrawSphere(pos, scaleAttractPoints);
		}
	}

	// What every atom does
	private void AtomBehaviour()
	{
		for(int i = 0; i < attractPoints.Length; ++i)
		{
			// Change Material color and therefore all atoms.
			Color atomColor;

			if (audioPlayback.getFreqSubbandsInstant()[attractPoints[i]] >= emissionThreshold)
			{
				atomColor = new Color(sharedColors[i].r * audioPlayback.getFreqSubbandsInstant()[i] * audioEmissionMultiplier,
										sharedColors[i].g * audioPlayback.getFreqSubbandsInstant()[i] * audioEmissionMultiplier,
										sharedColors[i].b * audioPlayback.getFreqSubbandsInstant()[i] * audioEmissionMultiplier, 1);
			}
			else
			{
				atomColor = defaultColor;
			}

			// If correct or incorrect input has occured, overwrite the atom color
			if (correctInput)
			{
				float timeElapsed = Time.time - correctEffectTime;

				if (timeElapsed >= inputEffectLength)
				{
					correctInput = false;
				}
				else
				{
					atomColor = Color.Lerp(correctInputColor, defaultColor, timeElapsed / inputEffectLength);
				}
			}
			else if (incorrectInput)
			{
				float timeElapsed = Time.time - incorrectEffectTime;

				if (timeElapsed >= inputEffectLength)
				{
					incorrectInput = false;
				}
				else
				{
					atomColor = Color.Lerp(incorrectInputColor, defaultColor, timeElapsed / inputEffectLength);
				}
			}

			sharedMaterials[i].SetColor("_EmissionColor", atomColor);

			for (int j = 0; j < numberOfAtoms; ++j)
			{
				int currentAtomCount = i * numberOfAtoms + j;

				// Change ScaleSet, and therefore the scale of all atoms
				atomList[currentAtomCount].transform.localScale = new Vector3(atomScaleSet[currentAtomCount] + audioPlayback.getFreqSubbandsInstant()[i] * audioScaleMultiplier,
					atomScaleSet[currentAtomCount] + audioPlayback.getFreqSubbandsInstant()[i] * audioScaleMultiplier,
					atomScaleSet[currentAtomCount] + audioPlayback.getFreqSubbandsInstant()[i] * audioScaleMultiplier);
			}
		}
	}
	#endregion
}
