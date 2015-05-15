package com.vungle;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.util.Log;

import com.vungle.publisher.*;
import com.vungle.publisher.inject.Injector;



public class VunglePlugin
{
	private static VunglePlugin _instance;
	protected static final String TAG = "Vungle";

	// this is the name of the GameObject that will receive all calls from Java to Unity
	protected static final String MANAGER_NAME = "VungleAndroidManager";
	// we leave this public in case we want to test a native Android app during development
	public Activity _activity;



	// ############# ###############
	// Private and Static Access
	// ############# ###############

	public static VunglePlugin instance()
	{
		if( _instance == null )
			_instance = new VunglePlugin();

		return _instance;
	}


	private Activity getActivity()
	{
		try
		{
			return com.unity3d.player.UnityPlayer.currentActivity;
		}
		catch( Exception e )
		{
			Log.i( TAG, "error getting currentActivity: " + e.getMessage() );
		}

		return _activity;
	}


	private void UnitySendMessage( String m, String p )
	{
		// ensure we have data here
		if( p == null )
			p = "";

		try
		{
			com.unity3d.player.UnityPlayer.UnitySendMessage( MANAGER_NAME, m, p );
		}
		catch( Exception e )
		{
			Log.i( TAG, "UnitySendMessage: " + MANAGER_NAME + ", " + m + ", " + p );
		}
	}


	private void runSafelyOnUiThread( final Runnable r )
	{
		getActivity().runOnUiThread( new Runnable()
		{
			@Override
			public void run()
			{
				try
				{
					r.run();
				}
				catch( Exception e )
				{
					Log.e( TAG, "Exception running command on UI thread: " + e.getMessage() );
				}
			}
		});
	}



	// ############# ###############
	// Public API
	// ############# ###############

	@SuppressLint( "DefaultLocale" )
	public void init( final String appId )
	{
		runSafelyOnUiThread( new Runnable()
		{
			@Override
			public void run()
			{
				// report plugin to Vungle
				final com.vungle.publisher.inject.Injector injector = Injector.getInstance();
				injector.setWrapperFramework( com.vungle.publisher.env.WrapperFramework.unity );
				injector.setWrapperFrameworkVersion( "2.1" );


				VunglePub.getInstance().init( getActivity(), appId );

				// setup our event listener
				VunglePub.getInstance().setEventListeners( new EventListener()
				{
					@Override
					public void onAdStart()
					{
						UnitySendMessage( "onAdStart", "" );
					}


					@Override
					public void onVideoView( boolean isCompletedView, int watchedMillis, int videoDurationMillis )
					{
						String str = String.format( "%d-%d", watchedMillis, videoDurationMillis );
						UnitySendMessage( "onVideoView", str );
					}


					@Override
					public void onAdUnavailable( String arg0 )
					{
						Log.i( TAG, "onAdUnavailable: " + arg0 );
					}


					@Override
					public void onAdEnd( boolean wasCallToActionClicked )
					{
						UnitySendMessage( "onAdEnd", wasCallToActionClicked ? "1" : "0" );
					}




					@Override
					public void onAdPlayableChanged( boolean isPlayable )
					{
						if( isPlayable )
							UnitySendMessage( "onCachedAdAvailable", "" );
					}
				} );

			}
		});
	}


	public void setAdOrientation( int orientation )
	{
		final AdConfig adConfig = VunglePub.getInstance().getGlobalAdConfig();

		if( orientation == 0 )
			adConfig.setOrientation( Orientation.autoRotate );
		else
			adConfig.setOrientation( Orientation.matchVideo );
	}


	public void onPause()
	{
		VunglePub.getInstance().onPause();
	}


	public void onResume()
	{
		VunglePub.getInstance().onResume();
	}


	public boolean isVideoAvailable()
	{
		return VunglePub.getInstance().isAdPlayable();
	}


	public void setSoundEnabled( boolean isEnabled )
	{
		final AdConfig adConfig = VunglePub.getInstance().getGlobalAdConfig();
		adConfig.setSoundEnabled( isEnabled );
	}


	public boolean isSoundEnabled()
	{
		final AdConfig adConfig = VunglePub.getInstance().getGlobalAdConfig();
		return adConfig.isSoundEnabled();
	}


	public void playAd( final boolean incentivized, final String user )
	{
		runSafelyOnUiThread( new Runnable()
		{
			@Override
			public void run()
			{
				final AdConfig config = new AdConfig();
				config.setIncentivized( incentivized );

				if( incentivized && user != null && user.length() > 0 )
					config.setIncentivizedUserId( user );

				VunglePub.getInstance().playAd( config );
			}
		});
	}

}
