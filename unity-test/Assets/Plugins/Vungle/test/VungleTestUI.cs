using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class VungleTestUI : MonoBehaviour
{
	public bool logView = false;
	public bool enableLog = true;
	public string log = "";
	public bool landscape = false;
	public bool portrait = false;
	public bool incentivized = false;
	public bool immersive = false;
	public bool muted = false;
	public bool large = false;
	public string appId = "vungleTest";
	public string userTag = "";
	public bool adAvailable = false;
	public string alertTitle = "";
	public string alertText = "";
	public string closeText = "";
	public string continueText = "";

	public string placement = "";
	public string key1 = "";
	public string key2 = "";
	public string key3 = "";
	public string key4 = "";
	public string key5 = "";
	public string key6 = "";
	public string key7 = "";
	public string key8 = "";

	public string endPoint = "";
	public bool bInit = false;
	public Vector2 scrollPosition = Vector2.zero;

	// Internal variables for managing touches and drags
	private int selected = -1;
	private float scrollVelocity = 0f;
	private float timeTouchPhaseEnded = 0f;
	
	public float inertiaDuration = 0.75f;
	// size of the window and scrollable list
	public int numRows = 100;
	public Vector2 rowSize = new Vector2(100, 50);
	public Vector2 windowMargin;
	public Vector2 listMargin;
	
	private Rect windowRect;   // calculated bounds of the window that holds the scrolling list
	private Vector2 listSize;  // calculated dimensions of the scrolling list placed inside the window

	void Update()
	{
		if (Input.touchCount != 1)
		{
			selected = -1;
			
			if ( scrollVelocity != 0.0f )
			{
				// slow down over time
				float t = (Time.time - timeTouchPhaseEnded) / inertiaDuration;
				if (scrollPosition.y <= 0 || scrollPosition.y >= (numRows*rowSize.y - listSize.y))
				{
					// bounce back if top or bottom reached
					scrollVelocity = -scrollVelocity;
				}
				
				float frameVelocity = Mathf.Lerp(scrollVelocity, 0, t);
				scrollPosition.y += frameVelocity * Time.deltaTime;
				
				// after N seconds, we've stopped
				if (t >= 1.0f) scrollVelocity = 0.0f;
			}
			return;
		}
		
		Touch touch = Input.touches[0];
		bool fInsideList = IsTouchInsideList(touch.position);
		
		if (touch.phase == TouchPhase.Began && fInsideList)
		{
			selected = TouchToRowIndex(touch.position);
			scrollVelocity = 0.0f;
		}
		else if (touch.phase == TouchPhase.Canceled || !fInsideList)
		{
			selected = -1;
		}
		else if (touch.phase == TouchPhase.Moved && fInsideList)
		{
			// dragging
			selected = -1;
			scrollPosition.y += touch.deltaPosition.y;
		}
		else if (touch.phase == TouchPhase.Ended)
		{
			// Was it a tap, or a drag-release?
			if ( selected > -1 && fInsideList )
			{
				Debug.Log("Player selected row " + selected);
			}
			else
			{
				// impart momentum, using last delta as the starting velocity
				// ignore delta < 10; precision issues can cause ultra-high velocity
				if (Mathf.Abs(touch.deltaPosition.y) >= 10) 
					scrollVelocity = (int)(touch.deltaPosition.y / touch.deltaTime);
				
				timeTouchPhaseEnded = Time.time;
			}
		}
		
	}

	Dictionary<string,object> formatOptions() {
		Dictionary<string,object> options = new Dictionary<string,object> ();
		#if UNITY_IPHONE
		var orientation = 6;
		if (landscape && !portrait) {
			orientation = 5;
		}
		else if (portrait)
			orientation = 7;
		options.Add ("orientation", orientation);
		#endif
		#if UNITY_ANDROID || UNITY_WSA_10_0 || UNITY_WINRT_8_1 || UNITY_METRO
		options.Add ("orientation", landscape?true:false);
		#endif
		options.Add ("incentivized", incentivized);
		options.Add ("large", large);
		if (userTag != "")
			options.Add ("userTag", userTag);
		if (alertTitle != "")
			options.Add ("alertTitle", alertTitle);
		if (alertText != "")
			options.Add ("alertText", alertText);
		if (closeText != "")
			options.Add ("closeText", closeText);
		if (continueText != "")
			options.Add ("continueText", continueText);
		options.Add ("placement", placement);
		options.Add ("immersive", immersive);
		options.Add ("key1", key1);
		options.Add ("key2", key2);
		options.Add ("key3", key3);
		options.Add ("key4", key4);
		options.Add ("key5", key5);
		options.Add ("key6", key6);
		options.Add ("key7", key7);
		options.Add ("key8", key8);
		return options;
	}

	void OnGUI()
	{
		beginGuiColomn();

		if( GUILayout.Button( logView?"Main":"Log" ) )
		{
			logView = !logView;
		}
		if (!logView) {
			#if UNITY_IPHONE
			GUI.enabled = !bInit;
			GUILayout.Label ("API Endpoint: ");
			endPoint = GUILayout.TextField (endPoint);
			if (GUILayout.Button ("Set endpoint")) {
				Vungle.setEndPoint (endPoint);
			}
			GUI.enabled = true;
			#endif
			GUILayout.Label ("App ID: ");
			appId = GUILayout.TextField (appId);
			GUILayout.Label ("User tag: ");
			userTag = GUILayout.TextField (userTag);

			if (GUILayout.Button ("Init")) {
				bInit = true;
				Vungle.init (appId, appId);
				if (Vungle.isAdvertAvailable ())
					adAvailable = true;
			}

			GUI.enabled = adAvailable;
		
			GUILayout.Label ("Options: ");

			#if UNITY_IPHONE
			landscape = GUILayout.Toggle (landscape, "Force landscape");
			portrait = GUILayout.Toggle (portrait, "Force portrait");
			#endif
			#if UNITY_ANDROID || UNITY_WSA_10_0 || UNITY_WINRT_8_1 || UNITY_METRO
			landscape = GUILayout.Toggle (landscape, "Match video orientation");
			immersive = GUILayout.Toggle (immersive, "Immersive mode");
			#endif
			incentivized = GUILayout.Toggle (incentivized, "Incentivized ad");
			muted = GUILayout.Toggle (muted, "Start muted");
			#if UNITY_IPHONE
			large = GUILayout.Toggle (large, "Large buttons");
			#endif

			if (GUILayout.Button ("Play Ad")) {
				Vungle.setSoundEnabled (!muted);
				Vungle.playAdWithOptions (formatOptions ());
			}

			#if UNITY_IPHONE
			if (GUILayout.Button ("Clear cache")) {
				adAvailable = false;
				Vungle.clearCache ();
			}
			if (GUILayout.Button ("Clear sleep")) {
				Vungle.clearSleep ();
			}
			#endif

			GUI.enabled = adAvailable && incentivized;
			GUILayout.Label ("Incentivized options: ");
			GUILayout.Label ("Alert title: ");
			alertTitle = GUILayout.TextField (alertTitle);
			GUILayout.Label ("Alert text: ");
			alertText = GUILayout.TextField (alertText);
			GUILayout.Label ("Alert 'Close' text: ");
			closeText = GUILayout.TextField (closeText);
			GUILayout.Label ("Alert 'Continue' test: ");
			continueText = GUILayout.TextField (continueText);

			GUI.enabled = adAvailable;
			GUILayout.Label ("More options: ");
			GUILayout.Label ("Placement: ");
			placement = GUILayout.TextField (placement);
			GUILayout.Label ("Key 1: ");
			key1 = GUILayout.TextField (key1);
			GUILayout.Label ("Key 2: ");
			key2 = GUILayout.TextField (key2);
			GUILayout.Label ("Key 3: ");
			key3 = GUILayout.TextField (key3);
			GUILayout.Label ("Key 4: ");
			key4 = GUILayout.TextField (key4);
			GUILayout.Label ("Key 5: ");
			key5 = GUILayout.TextField (key5);
			GUILayout.Label ("Key 6: ");
			key6 = GUILayout.TextField (key6);
			GUILayout.Label ("Key 7: ");
			key7 = GUILayout.TextField (key7);
			GUILayout.Label ("Key 8: ");
			key8 = GUILayout.TextField (key8);
		} else {
			if (GUILayout.Button ("Clear log")) {
				log = "";
			}
			#if UNITY_IPHONE
			if (GUILayout.Button (enableLog ? "Disable log" : "Enable log")) {
				enableLog = !enableLog;
				Vungle.setLogEnable(enableLog);
			}
			#endif
			GUILayout.Label (log);
		}
		endGuiColumn();
	}

	#region Optional: Example of Subscribing to All Events

	void OnEnable()
	{
		Vungle.onAdStartedEvent += onAdStartedEvent;
		Vungle.adPlayableEvent += adPlayableEvent;
		Vungle.onAdFinishedEvent += onAdFinishedEvent;
		Vungle.onLogEvent += onLogEvent;
	}


	void OnDisable()
	{
		Vungle.onAdStartedEvent -= onAdStartedEvent;
		Vungle.adPlayableEvent -= adPlayableEvent;
		Vungle.onAdFinishedEvent -= onAdFinishedEvent;
		Vungle.onLogEvent -= onLogEvent;
	}
	
	void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus)
			Vungle.onPause();
		else
			Vungle.onResume();
	}


	void onAdStartedEvent()
	{
		log = "onAdStartedEvent\n" + log;
		Debug.Log( "onAdStartedEvent" );
	}


	void adPlayableEvent(bool available)
	{
		log = "adPlayableEvent: " + available + "\n" + log;
		adAvailable = available;
		Debug.Log( "onCachedAdAvailableEvent" );
	}
	
	void onAdFinishedEvent(AdFinishedEventArgs arg)
	{
		log = "onAdFinishedEvent. watched: " + arg.TimeWatched + ", length: " + arg.TotalDuration  + ", isCompletedView: " + arg.IsCompletedView + "\n" + log;
		Debug.Log( "onAdFinishedEvent" );
	}

	void onLogEvent(string logMessage)
	{
		log = logMessage + "\n" + log;
		Debug.Log( "onLogEvent" );
	}
	
	#endregion

	void Start () {
		endPoint = Vungle.getEndPoint ();
		if (endPoint == null || endPoint == "") 
			endPoint = "http://api.vungle.com/api/v3";
	}

	#region Helpers to Tame GUI

	void beginGuiColomn()
	{
		windowRect = new Rect(windowMargin.x, windowMargin.y, 
		                      Screen.width - (2*windowMargin.x), Screen.height - (2*windowMargin.y));
		listSize = new Vector2(windowRect.width - 2*listMargin.x, windowRect.height - 2*listMargin.y);

		var buttonHeight = ( Screen.width >= 960 || Screen.height >= 960 ) ? 100 : 40;
		var buttonWidth = Screen.width - 40;
		var margin = new RectOffset( 0, 0, 10, 0 );
		var fontSize = ( Screen.width >= 960 || Screen.height >= 960 ) ? 35 : 16;

		//GUI.skin.label.fixedHeight = buttonHeight;
		//GUI.skin.label.margin = margin;
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.skin.label.fontSize = fontSize;

		GUI.skin.button.margin = margin;
		GUI.skin.button.fixedHeight = buttonHeight;
		GUI.skin.button.fixedWidth = buttonWidth;
		GUI.skin.button.wordWrap = true;
		GUI.skin.button.fontSize = fontSize;

		GUI.skin.textField.margin = margin;
		GUI.skin.textField.fixedWidth = buttonWidth;
		GUI.skin.textField.fixedHeight = buttonHeight*(float)0.8;
		GUI.skin.textField.fontSize = fontSize;

		GUI.skin.toggle.fontSize = fontSize;

		GUILayout.BeginArea( new Rect( 10, 10, Screen.width - 20, Screen.height - 20 ) );
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height (Screen.height - 20));
	}


	void endGuiColumn( bool hasSecondColumn = false )
	{
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	#endregion

	private int TouchToRowIndex(Vector2 touchPos)
	{
		float y = Screen.height - touchPos.y;  // invert y coordinate
		y += scrollPosition.y;  // adjust for scroll position
		y -= windowMargin.y;    // adjust for window y offset
		y -= listMargin.y;      // adjust for scrolling list offset within the window
		int irow = (int)(y / rowSize.y);
		
		irow = Mathf.Min(irow, numRows);  // they might have touched beyond last row
		return irow;
	}
	
	bool IsTouchInsideList(Vector2 touchPos)
	{
		Vector2 screenPos    = new Vector2(touchPos.x, Screen.height - touchPos.y);  // invert y coordinate
		Rect rAdjustedBounds = new Rect(listMargin.x + windowRect.x, listMargin.y + windowRect.y, listSize.x, listSize.y);
		
		return rAdjustedBounds.Contains(screenPos);
	}
}
