package xpresscode;


public class DownAnimatorListenerAdapter
	extends android.animation.AnimatorListenerAdapter
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onAnimationEnd:(Landroid/animation/Animator;)V:GetOnAnimationEnd_Landroid_animation_Animator_Handler\n" +
			"";
		mono.android.Runtime.register ("XpressCode.DownAnimatorListenerAdapter, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DownAnimatorListenerAdapter.class, __md_methods);
	}


	public DownAnimatorListenerAdapter () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DownAnimatorListenerAdapter.class)
			mono.android.TypeManager.Activate ("XpressCode.DownAnimatorListenerAdapter, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public DownAnimatorListenerAdapter (xpresscode.SwipeDismissListViewTouchListener p0, android.view.View p1, int p2) throws java.lang.Throwable
	{
		super ();
		if (getClass () == DownAnimatorListenerAdapter.class)
			mono.android.TypeManager.Activate ("XpressCode.DownAnimatorListenerAdapter, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "XpressCode.SwipeDismissListViewTouchListener, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void onAnimationEnd (android.animation.Animator p0)
	{
		n_onAnimationEnd (p0);
	}

	private native void n_onAnimationEnd (android.animation.Animator p0);

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
