using System;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Android.Graphics;
using Android.Animation;
using Java.Util;
using Android.OS;

namespace XpressCode
{
	public interface IDismissCallbacks
	{
		/**
         * Called to determine whether the given position can be dismissed.
         */
		bool canDismiss (int position);

		/**
         * Called when the user has indicated they she would like to dismiss one or more list item
         * positions.
         *
         * @param listView               The originating {@link ListView}.
         * @param reverseSortedPositions An array of positions to dismiss, sorted in descending
         *                               order for convenience.
         */
		void onDismiss (ListView listView, int[] reverseSortedPositions);
	}

	public class SwipeDismissListViewTouchListener:Java.Lang.Object,Android.Views.View.IOnTouchListener
	{
		// Cached ViewConfiguration and system-wide constant values
		int mSlop;
		int mMinFlingVelocity;
		int mMaxFlingVelocity;
		long mAnimationTime;

		// Fixed properties
		internal ListView mListView;
		internal IDismissCallbacks mCallbacks;
		internal int mViewWidth = 1;

	
		// Transient properties
		internal List<PendingDismissData> mPendingDismisses = new List<PendingDismissData> ();
		internal int mDismissAnimationRefCount = 0;
		internal float mDownX;
		internal float mDownY;
		internal bool mSwiping;
		internal int mSwipingSlop; 
		internal VelocityTracker mVelocityTracker;
		internal int mDownPosition;
		internal View mDownView;
		internal bool mPaused;

		public SwipeDismissListViewTouchListener (ListView listView, IDismissCallbacks callbacks)
		{
			ViewConfiguration vc = ViewConfiguration.Get (listView.Context);
			mSlop = vc.ScaledTouchSlop;
			mMinFlingVelocity = vc.ScaledMinimumFlingVelocity * 16;
			mMaxFlingVelocity = vc.ScaledMaximumFlingVelocity;
			mAnimationTime = listView.Context.Resources.GetInteger (Android.Resource.Integer.ConfigShortAnimTime);
			mListView = listView;
			mCallbacks = callbacks;
		}

		/**
     * Enables or disables (pauses or resumes) watching for swipe-to-dismiss gestures.
     *
     * @param enabled Whether or not to watch for gestures.
     */
		public void SetEnabled (bool enabled)
		{
			mPaused = !enabled;
		}


		/**
     * Returns an {@link android.widget.AbsListView.OnScrollListener} to be added to the {@link
     * ListView} using {@link ListView#setOnScrollListener(android.widget.AbsListView.OnScrollListener)}.
     * If a scroll listener is already assigned, the caller should still pass scroll changes through
     * to this listener. This will ensure that this {@link SwipeDismissListViewTouchListener} is
     * paused during list view scrolling.</p>
     *
     * @see SwipeDismissListViewTouchListener
     */
		public AbsListView.IOnScrollListener MakeScrollListener() {
		
			return new OnScrollListener (this);
		}

		class OnScrollListener:AbsListView.IOnScrollListener
		{
			SwipeDismissListViewTouchListener _listener;
			public OnScrollListener(SwipeDismissListViewTouchListener listener)
			{
				_listener = listener;
			}

			public void OnScroll (AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
			{

			}
			public void OnScrollStateChanged (AbsListView view, ScrollState scrollState)
			{
				_listener.SetEnabled(scrollState != ScrollState.TouchScroll);
			}
			public void Dispose ()
			{
				throw new NotImplementedException ();
			}
			public IntPtr Handle {
				get { 
					return new IntPtr ();
				}
			}
		}



		public bool OnTouch(View view, MotionEvent motionEvent)
		{
			if (mViewWidth < 2) {
				mViewWidth = mListView.Width;
			}

			switch (motionEvent.ActionMasked) {
			case MotionEventActions.Down:
				if (mPaused) {
					return false;
				}
				// TODO: ensure this is a finger, and set a flag
				// Find the child view that was touched (perform a hit test)
				Rect rect = new Rect ();
				int childCount = mListView.ChildCount;
				int[] listViewCoords = new int[2];
				mListView.GetLocationOnScreen (listViewCoords);
				int x = (int)motionEvent.RawX - listViewCoords [0];
				int y = (int)motionEvent.RawY - listViewCoords [1];
				View child;
				for (int i = 0; i < childCount; i++) {
					child = mListView.GetChildAt (i);
					child.GetHitRect (rect);
					if (rect.Contains (x, y)) {
						mDownView = child;
						break;
					}
				}

				if (mDownView != null) {
					mDownX = motionEvent.RawX;
					mDownY = motionEvent.RawY;
					mDownPosition = mListView.GetPositionForView (mDownView);
					if (mCallbacks.canDismiss (mDownPosition)) {
						mVelocityTracker = VelocityTracker.Obtain ();
						mVelocityTracker.AddMovement (motionEvent);
					} else {
						mDownView = null;
					}
				}
				return false;

			case MotionEventActions.Cancel: {
					if (mVelocityTracker == null) {
						break;
					}

					if (mDownView != null && mSwiping) {
						// cancel
						mDownView.Animate()
							.TranslationX(0)
							.Alpha(1)
							.SetDuration(mAnimationTime)
							.SetListener(null);
					}
					mVelocityTracker.Recycle();
					mVelocityTracker = null;
					mDownX = 0;
					mDownY = 0;
					mDownView = null;
					mDownPosition = ListView.InvalidPosition;
					mSwiping = false;
					break;
				}

			case MotionEventActions.Up: {
					if (mVelocityTracker == null) {
						break;
					}

					float deltaX = motionEvent.RawX - mDownX;
					mVelocityTracker.AddMovement(motionEvent);
					mVelocityTracker.ComputeCurrentVelocity(1000);
					float velocityX = mVelocityTracker.XVelocity;
					float absVelocityX = Math.Abs(velocityX);
					float absVelocityY = Math.Abs(mVelocityTracker.YVelocity);
					bool dismiss = false;
					bool dismissRight = false;
					if (Math.Abs(deltaX) > mViewWidth / 2) {
						dismiss = true;
						dismissRight = deltaX > 0;
					} else
						if (mMinFlingVelocity <= absVelocityX && absVelocityX <= mMaxFlingVelocity&& absVelocityY < absVelocityX&&mSwiping) {
						// dismiss only if flinging in the same direction as dragging
						dismiss = (velocityX < 0) == (deltaX < 0);
						dismissRight = mVelocityTracker.XVelocity > 0;
					}
					if (dismiss&& mDownPosition != ListView.InvalidPosition) {
						// dismiss
						View downView = mDownView; // mDownView gets null'd before animation ends
						int downPosition = mDownPosition;
						++mDismissAnimationRefCount;
						var anim = mDownView.Animate ()
							.TranslationX (dismissRight ? mViewWidth : -mViewWidth)
							.Alpha (0)
							.SetDuration (mAnimationTime)
							.SetListener (new DownAnimatorListenerAdapter(this,downView,downPosition));
					} 

					else {
						// cancel
						mDownView.Animate()
							.TranslationX(0)
							.Alpha(1)
							.SetDuration(mAnimationTime)
							.SetListener(null);
					}
					mVelocityTracker.Recycle();
					mVelocityTracker = null;
					mDownX = 0;
					mDownY = 0;
					mDownView = null;
					mDownPosition = ListView.InvalidPosition;
					mSwiping = false;
					break;
				}

			case MotionEventActions.Move: {
					if (mVelocityTracker == null || mPaused) {
						break;
					}

					mVelocityTracker.AddMovement(motionEvent);
					float deltaX = motionEvent.RawX - mDownX;
					float deltaY = motionEvent.RawY - mDownY;
					if (Math.Abs(deltaX) > mSlop && Math.Abs(deltaY) < Math.Abs(deltaX) / 2)  {
						mSwiping = true;
						mSwipingSlop = (deltaX > 0 ? mSlop : -mSlop);
						mListView.RequestDisallowInterceptTouchEvent(true);

						// Cancel ListView's touch (un-highlighting the item)
						MotionEvent cancelEvent = MotionEvent.Obtain(motionEvent);
						cancelEvent.Action= (Android.Views.MotionEventActions) ((int) MotionEventActions.Cancel |
							((int)motionEvent.ActionIndex <<(int) MotionEventActions.PointerIndexShift));
						mListView.OnTouchEvent(cancelEvent);
						cancelEvent.Recycle();
					}

					if (mSwiping) {
						mDownView.TranslationX=deltaX - mSwipingSlop;
						mDownView.Alpha=Math.Max(0f, Math.Min(1f,1f - 2f * Math.Abs(deltaX) / mViewWidth));
						return true;
					}
					break;
				}
			}
			return false;
		}



		/**
     * Manually cause the item at the given position to be dismissed (trigger the dismiss
     * animation).
     */


		public void PerformDismiss(View dismissView, int dismissPosition) {
			// Animate the dismissed list item to zero-height and fire the dismiss callback when
			// all dismissed list item animations have completed. This triggers layout on each animation
			// frame; in the future we may want to do something smarter and more performant.
			ViewGroup.LayoutParams lp = dismissView.LayoutParameters;
			int originalHeight = dismissView.Height;
			ValueAnimator animator = ValueAnimator.OfInt (originalHeight, 1);
			animator.SetDuration(mAnimationTime);
			animator.AddListener (new DismissAnimatorListenerAdapter (this,originalHeight));

			animator.AddUpdateListener(new AnimatorUpdateListener(dismissView,lp));

			mPendingDismisses.Add(new PendingDismissData(dismissPosition, dismissView));
			animator.Start();
		}

		public void DismissAnimationEnded(int originalHeight)
		{
			--mDismissAnimationRefCount;
			if (mDismissAnimationRefCount == 0) {
				// No active animations, process all pending dismisses.
				// Sort by descending position
				mPendingDismisses.Sort ();
				//Collections.Sort(mPendingDismisses);

				int[] dismissPositions = new int[mPendingDismisses.Count];
				for (int i = mPendingDismisses.Count - 1; i >= 0; i--) {
					dismissPositions[i] = mPendingDismisses[i].position;
				}
				mCallbacks.onDismiss(mListView, dismissPositions);

				// Reset mDownPosition to avoid MotionEvent.ACTION_UP trying to start a dismiss 
				// animation with a stale position
				mDownPosition = ListView.InvalidPosition;


				foreach (PendingDismissData pendingDismiss in mPendingDismisses) {
					// Reset view presentation
					pendingDismiss.view.Alpha=1f;
					pendingDismiss.view.TranslationX=0;
					var cc = pendingDismiss.view.LayoutParameters;
					cc.Height = originalHeight;
					pendingDismiss.view.LayoutParameters=cc;
				}

				// Send a cancel event
				long time = SystemClock.UptimeMillis();
				MotionEvent cancelEvent = MotionEvent.Obtain(time, time,MotionEventActions.Cancel, 0, 0, 0);
				mListView.DispatchTouchEvent(cancelEvent);

				mPendingDismisses.Clear();
			}
		}
	}

	class AnimatorUpdateListener : Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener
	{
		View dismissView;
		ViewGroup.LayoutParams lp;
		public AnimatorUpdateListener (View dismissView, ViewGroup.LayoutParams lp)
		{
			this.dismissView = dismissView;
			this.lp = lp;
		}

		public void OnAnimationUpdate (ValueAnimator valueAnimator)
		{
			lp.Height = (int) valueAnimator.AnimatedValue;
			dismissView.LayoutParameters=lp;
		}
	}

	class DownAnimatorListenerAdapter:AnimatorListenerAdapter
	{
		View _downView; // mDownView gets null'd before animation ends
		int _downPosition ;
		SwipeDismissListViewTouchListener _parent;
		public DownAnimatorListenerAdapter(SwipeDismissListViewTouchListener parent,View downView,int downPosition)
		{
			_downView = downView;
			_parent = parent;
			_downPosition = downPosition;
		}

		public override void OnAnimationEnd (Animator animation)
		{
			base.OnAnimationEnd (animation);
			_parent.PerformDismiss (_downView, _downPosition);
		}
	}


	class DismissAnimatorListenerAdapter:AnimatorListenerAdapter
	{
		SwipeDismissListViewTouchListener _parent;
		int _originalHeight;
		public DismissAnimatorListenerAdapter(SwipeDismissListViewTouchListener parent,int originalHeight)
		{
			_parent = parent;
			_originalHeight = originalHeight;
		}

		public override void OnAnimationEnd (Animator animation)
		{
			base.OnAnimationEnd (animation);
			_parent.DismissAnimationEnded (_originalHeight);
		}
	}


	class PendingDismissData:IComparable<PendingDismissData> 
	{
		public int position;
		public View view;

		public PendingDismissData(int position, View view) {
			this.position = position;
			this.view = view;
		}

		public int CompareTo(PendingDismissData other) {
			// Sort by descending position
			return other.position - position;
		}
	}

}

