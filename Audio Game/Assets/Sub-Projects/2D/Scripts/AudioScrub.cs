//using unityengine;
//using system.collections;

//public class audioscrub : monobehaviour
//{

//	gameobject audiofrom;//used to hold the audiosource
//	gameobject audiocompiler;
//	private float scrollpos = 0f;//position of scroll
//	bool trig;// used as a "gate"
//	public string audioobject; // this is the game object which holds the audio source
//	public string audiocompilerobject;
//	public audioclip song;
//	audiosource compiler, player;
//	public float audioplayerdelay = 3.0f;

//	float currenttime;
//	bool paused;
//	float timeatstartofgame;

//	private void start()
//	{
//		timeatstartofgame = time.time;
//		audiofrom = gameobject.find(audioobject);//get audio from game object
//		audiocompiler = gameobject.find(audiocompilerobject);
//		player = audiofrom.getcomponent<audiosource>();
//		player.clip = song;
//		player.pause();
//		paused = true;
//		compiler = audiocompiler.getcomponent<audiosource>();

//		compiler.clip = song;
//		compiler.play();
//	}

//	void update()
//	{
//		if (paused) checktimeandunpause();
//	}

//	void checktimeandunpause()
//	{
//		currenttime = time.time - timeatstartofgame;
//		if (currenttime >= audioplayer.playdelay)
//		{
//			player.play();
//			paused = false;
//		}
//	}

//	private void ongui()
//	{

//		scrollpos = gui.horizontalslider(new rect(0f, 50f, screen.width, 50f), scrollpos, 0, audiofrom.getcomponent<audiosource>().clip.length);

//		if (gui.changed == true)
//		{
//			trig = true;// open "gate"
//		}
//		if (gui.changed == false && !input.getmousebutton(0) && trig == false)
//		{
//			scrollpos = audiofrom.getcomponent<audiosource>().time;// makes slider follow the audio when not used (clicked)
//		}
//		if (input.getmousebuttonup(0) && trig == true)
//		{
//			audiofrom.getcomponent<audiosource>().time = scrollpos;// will only change the audio position once the mouse is released
//			audiocompiler.getcomponent<audiosource>().time = scrollpos + audiocompiler.getcomponent<audiocompiler>().playdelay;
//			trig = false;
//		}

//		gui.label(new rect(10f, 80f, 100f, 30f), (audiofrom.getcomponent<audiosource>().time).tostring());

//	}
//}