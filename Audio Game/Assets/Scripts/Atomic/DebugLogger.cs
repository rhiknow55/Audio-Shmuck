using UnityEngine;
using TMPro;

public class DebugLogger : MonoBehaviour 
{
	public static DebugLogger instance;

	[SerializeField]
	private TextMeshProUGUI _label;

	void Awake()
	{
		// Check if instance already exists
		if (instance == null)
			// if not, set instance to this
			instance = this;
		// If instance already exists (not null) and it is not this
		else if (instance != this)
			// Then destroy this gameobject. This enforces our singleton pattern, meaning there can only ever be one instance of this manager
			Destroy(this.gameObject);
	}

	public void Log(string text)
	{
		_label.text += "\n" + text;
	}
}
