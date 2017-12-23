using UnityEngine;

public class AtomicAttraction : MonoBehaviour 
{
	#region Variables
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
	
	
	private GameObject[] attractorList, atomList;
	private float[] atomScaleSet;
	#endregion

	#region Monobehaviour Methods
	void Start()
	{
		attractorList = new GameObject[attractPoints.Length];
		atomList = new GameObject[attractPoints.Length * numberOfAtoms];
		atomScaleSet = new float[attractPoints.Length * numberOfAtoms];
		
		// Instantiate all the attractors
		for(int i = 0; i < attractPoints.Length; ++i)
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
				}else
				{
					atomInstance.GetComponent<Rigidbody>().useGravity = false;
				}

				atomInstance.transform.parent = atomContainer.transform;
			}
		}
	}
	#endregion

	#region Public Methods
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
	#endregion
}
