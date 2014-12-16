using System;
using Android.Views;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.Text;
using Android.Runtime;
using Android.Animation;
using Java.Lang;
using Newtonsoft.Json;
using System.Collections.Generic;
using SwipeDismissListView;

namespace XpressCode
{
	public interface IUndoListener
	{
		void OnUndo (object obj);
	}

	public class UndoBarController
	{
		View mBarView;
		TextView mMessageView;
		ViewPropertyAnimator mBarAnimator;
		Handler mHideHandler = new Handler ();
		internal IUndoListener mUndoListener;

		// State objects
		internal object mUndoToken;
		string mUndoMessage;

		public UndoBarController (View undoBarView, IUndoListener undoListener)
		{
			mBarView = undoBarView;
			mBarAnimator = mBarView.Animate ();
			mUndoListener = undoListener;

			mMessageView = (TextView)mBarView.FindViewById (Resource.Id.undobar_message);
			mBarView.FindViewById<Button> (Resource.Id.undobar_button).Click += (object sender, EventArgs e) => {
				HideUndoBar (false);
				mUndoListener.OnUndo (mUndoToken);
			};
			HideUndoBar (true);
		}

		public void ShowUndoBar (bool immediate, string message, object undoToken)
		{
			mUndoToken = undoToken;
			mUndoMessage = message;
			mMessageView.Text = mUndoMessage;


			mBarView.Visibility = ViewStates.Visible;
			if (immediate) {
				mBarView.Alpha = 1;
			} else {
				mBarAnimator.Cancel ();
				mBarAnimator
					.Alpha (1)
					.SetDuration (mBarView.Resources.GetInteger (Android.Resource.Integer.ConfigShortAnimTime))
					.SetListener (null);
			}

			t = new System.Timers.Timer (mBarView.Resources.GetInteger (Resource.Integer.undobar_hide_delay));
			t.Elapsed += HandleTimeoutElapsed;
			t.Start ();

		}


		void HandleTimeoutElapsed (object sender, System.Timers.ElapsedEventArgs e)
		{
			DisposeTimer ();
			HideUndoBar (false);
		}

		public void DisposeTimer()
		{
			lock(this)
			{
				if (t==null)
					return;
				t.Stop ();
				t.Elapsed -= HandleTimeoutElapsed;
				t.Dispose ();
				t = null;
			}
		}

		System.Timers.Timer t;

		public void HideUndoBar (bool immediate)
		{
			DisposeTimer ();
			if (immediate) {
				mBarView.Visibility = ViewStates.Gone;
				mBarView.Alpha = 0;
				mUndoMessage = null;
				mUndoToken = null;

			} else {
				mBarAnimator.Cancel ();
				var animator = mBarAnimator
					.Alpha (0)
					.SetDuration (mBarView.Resources.GetInteger (Android.Resource.Integer.ConfigShortAnimTime))
					.SetListener (new CloseAnimatorListenerAdapter (this));

			}
		}

		public void ClosePopup ()
		{
			mBarView.Visibility = ViewStates.Gone;
			mUndoMessage = null;
			mUndoToken = null;
		}

		public void onSaveInstanceState (Bundle outState)
		{
			outState.PutCharSequence ("undo_message", mUndoMessage);
			outState.PutString ("undo_token", JsonConvert.SerializeObject (mUndoToken));
		}

		public void onRestoreInstanceState (Bundle savedInstanceState)
		{
			if (savedInstanceState != null) {
				mUndoMessage = savedInstanceState.GetCharSequence ("undo_message");
				mUndoToken = JsonConvert.DeserializeObject<object> (savedInstanceState.GetString ("undo_token"));

				if (mUndoToken != null || !TextUtils.IsEmpty (mUndoMessage)) {
					ShowUndoBar (true, mUndoMessage, mUndoToken);
				}
			}
		}
	}

	class CloseAnimatorListenerAdapter:AnimatorListenerAdapter
	{
		UndoBarController controller;

		public CloseAnimatorListenerAdapter (UndoBarController controller)
		{
			this.controller = controller;
		}

		public override void OnAnimationEnd (Animator animation)
		{
			controller.HideUndoBar (false);
		}
	}


}

