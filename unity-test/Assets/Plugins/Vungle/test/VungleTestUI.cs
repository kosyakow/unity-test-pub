using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class VungleTestUI : MonoBehaviour
{
	public bool landscape = false;
	public bool portrait = false;
	public bool incentivized = false;
	public bool muted = false;
	public bool large = false;
	public string appId = "vungleTest";
	public bool adAvailable = false;
	public string alertTitle = "";
	public string alertText = "";
	public string closeText = "";
	public string continueText = "";
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

#if UNITY_IPHONE || UNITY_ANDROID

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

	void OnGUI()
	{
		beginGuiColomn();

		#if UNITY_IPHONE
		GUI.enabled = !bInit;
		GUILayout.Label ("API Endpoint: ");
		endPoint = GUILayout.TextField (endPoint);
		if( GUILayout.Button( "Set endpoint" ) )
		{
			Vungle.setEndPoint(endPoint);
		}
		GUI.enabled = true;
		#endif
		GUILayout.Label ("App ID: ");
		appId = GUILayout.TextField (appId);

		if( GUILayout.Button( "Init" ) )
		{
			bInit = true;
			Vungle.init( appId, appId );
			if (Vungle.isAdvertAvailable())
				adAvailable = true;
		}

		GUI.enabled = adAvailable;
		
		GUILayout.Label ("Options: ");

		#if UNITY_IPHONE
		landscape = GUILayout.Toggle(landscape, "Force landscape");
		portrait = GUILayout.Toggle(portrait, "Force portrait");
		#endif
		incentivized = GUILayout.Toggle(incentivized, "Incentivized ad");
		muted = GUILayout.Toggle(muted, "Start muted");
		#if UNITY_IPHONE
		large = GUILayout.Toggle(large, "Large buttons");
		#endif

		if( GUILayout.Button( "Play Ad" ) )
		{
			Vungle.setSoundEnabled( !muted );
			var orientation = 5;
			if (landscape && !portrait) {
				orientation = 4;
			}
			else if (portrait)
				orientation = 3;
			Vungle.playAdEx(incentivized, orientation, large, "", alertTitle, alertText, closeText, continueText);
		}

		#if UNITY_IPHONE
		if( GUILayout.Button( "Clear cache" ) )
		{
			adAvailable = false;
			Vungle.clearCache( );
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
		endGuiColumn();
	}

	#region Optional: Example of Subscribing to All Events

	void OnEnable()
	{
		Vungle.onAdStartedEvent += onAdStartedEvent;
		Vungle.onAdEndedEvent += onAdEndedEvent;
		Vungle.onAdViewedEvent += onAdViewedEvent;
		Vungle.onCachedAdAvailableEvent += onCachedAdAvailableEvent;
	}


	void OnDisable()
	{
		Vungle.onAdStartedEvent -= onAdStartedEvent;
		Vungle.onAdEndedEvent -= onAdEndedEvent;
		Vungle.onAdViewedEvent -= onAdViewedEvent;
		Vungle.onCachedAdAvailableEvent -= onCachedAdAvailableEvent;
	}


	void onAdStartedEvent()
	{
		Debug.Log( "onAdStartedEvent" );
	}


	void onAdEndedEvent()
	{
		Debug.Log( "onAdEndedEvent" );
	}


	void onAdViewedEvent( double watched, double length )
	{
		Debug.Log( "onAdViewedEvent. watched: " + watched + ", length: " + length );
	}


	void onCachedAdAvailableEvent()
	{
		adAvailable = true;
		Debug.Log( "onCachedAdAvailableEvent" );
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

#endif
}
