using UnityEngine;

public class AttractTo : MonoBehaviour 
{
	#region Variables


	//The strength of the force of attraction. (Use this to multiply when Addforce)
	private float strengthOfAttraction;
	//Max magnitude of the velocity this Atom can reach.
	private float maxMagnitude;
	// The transform of the attractor that this atom is assigned to.
	private Transform attractedTo;
	Rigidbody _rigidbody;
	#endregion

	#region Monobehaviour Methods
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if(attractedTo != null)
		{
			Vector3 direction = attractedTo.position - transform.position;
			_rigidbody.AddForce(strengthOfAttraction * direction);

			if(_rigidbody.velocity.magnitude > maxMagnitude)
			{
				_rigidbody.velocity = _rigidbody.velocity.normalized * maxMagnitude;
			}
		}
	}
	#endregion

	#region Public Methods
	/// <summary>
	/// Set up the atom with its attractor, max magnitude of velocity, and strength of force of attraction.
	/// </summary>
	/// <param name="_attractedTo"></param>
	/// <param name="_maxMagnitude"></param>
	/// <param name="_strengthOfAttraction"></param>
	public void SetupAtom(Transform _attractedTo, float _maxMagnitude, float _strengthOfAttraction)
	{
		attractedTo = _attractedTo;
		maxMagnitude = _maxMagnitude;
		strengthOfAttraction = _strengthOfAttraction;
	}
	#endregion

	#region Private/Protected Methods
	#endregion
}
