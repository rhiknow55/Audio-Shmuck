using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongChoiceMenu : MonoBehaviour {

	public void OpenFolderPanel()
	{
		StartCoroutine(SongChoiceMenuEditor.DesignateMusicFolder());

		
	}
}
