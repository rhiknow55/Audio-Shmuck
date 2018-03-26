using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongChoiceView : MonoBehaviour
{
	public Action onConvertedClip;

	[SerializeField]
	private Button _button;

	[SerializeField]
	private TextMeshProUGUI _songLabel;

	[SerializeField]
	private TMP_Dropdown _dropDown;

	private void Start()
	{
		_button.onClick.AddListener(HandleButtonPress);

		onConvertedClip += HandleClipConverted;
		_dropDown.onValueChanged.AddListener(HandleValueChanged);
	}

	private void OnDestroy()
	{
		_button.onClick.RemoveListener(HandleButtonPress);
		onConvertedClip -= HandleClipConverted;
	}

	private void HandleButtonPress()
	{
		StartCoroutine(SongChoiceMenuEditor.DesignateMusicFolder(onConvertedClip));
	}

	private void HandleClipConverted()
	{
		AudioCompiler.instance.Init();
		AudioPlayback.instance.Init();

		// Update dropdown
		List<AudioClip> songs = SongManager.instance.songs;

		List<string> names = new List<string>();
		for (int i = 0; i < songs.Count; ++i)
		{
			string fileName = songs[i].name;
			int startIndex = fileName.LastIndexOf('\\') + 1;
			int length = fileName.LastIndexOf('.') - startIndex;
			string name = fileName.Substring(startIndex, length);

			names.Add(name);
		}

		_dropDown.AddOptions(names);
	}
	
	private void HandleValueChanged(int index)
	{
		AudioClip chosenClip = SongManager.instance.songs[index];

		SongManager.instance.SetSelectedSong(chosenClip);

		_songLabel.text = "Current song: " + chosenClip.name;
		SongManager.instance.PlaySong();
	}
}

