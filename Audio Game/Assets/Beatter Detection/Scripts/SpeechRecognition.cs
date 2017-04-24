using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class SpeechRecognition : MonoBehaviour {

	KeywordRecognizer keywordRecognizer;
	Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
	List<string> words = new List<string>();

	void Start() {
		words.Add("go");
		words.Add("attack");
		words.Add("1");

		for (int i = 0; i < words.Count; i++) {
			AddKeyWord(words[i]);
		}

		keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
		keywordRecognizer.OnPhraseRecognized += KeywordRecognizerOnPhraseRecognized;
		keywordRecognizer.Start();
	}

	void KeywordRecognizerOnPhraseRecognized(PhraseRecognizedEventArgs args) {
		System.Action keywordAction;

		if(keywords.TryGetValue(args.text, out keywordAction)) {
			keywordAction.Invoke();
		}
	}

	void AddKeyWord(string word) {
		keywords.Add(word, () => {
			KeyWordCalled(word);
		});
	}

	void KeyWordCalled(string word) {
		print("You just said " + word);
	}
}
