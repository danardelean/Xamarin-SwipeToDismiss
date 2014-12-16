package xpresscode;


public class AnimatorUpdateListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.animation.ValueAnimator.AnimatorUpdateListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onAnimationUpdate:(Landroid/animation/ValueAnimator;)V:GetOnAnimationUpdate_Landroid_animation_ValueAnimator_Handler:Android.Animation.ValueAnimator/IAnimatorUpdateListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("XpressCode.AnimatorUpdateListener, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AnimatorUpdateListener.class, __md_methods);
	}


	public AnimatorUpdateListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AnimatorUpdateListener.class)
			mono.android.TypeManager.Activate ("XpressCode.AnimatorUpdateListener, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public AnimatorUpdateListener (android.view.View p0, android.view.ViewGroup.LayoutParams p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == AnimatorUpdateListener.class)
			mono.android.TypeManager.Activate ("XpressCode.AnimatorUpdateListener, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Views.ViewGroup/LayoutParams, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public void onAnimationUpdate (android.animation.ValueAnimator p0)
	{
		n_onAnimationUpdate (p0);
	}

	private native void n_onAnimationUpdate (android.animation.ValueAnimator p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
