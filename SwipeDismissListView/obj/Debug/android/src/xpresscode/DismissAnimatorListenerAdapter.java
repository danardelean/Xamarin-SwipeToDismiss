package xpresscode;


public class DismissAnimatorListenerAdapter
	extends android.animation.AnimatorListenerAdapter
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onAnimationEnd:(Landroid/animation/Animator;)V:GetOnAnimationEnd_Landroid_animation_Animator_Handler\n" +
			"";
		mono.android.Runtime.register ("XpressCode.DismissAnimatorListenerAdapter, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DismissAnimatorListenerAdapter.class, __md_methods);
	}


	public DismissAnimatorListenerAdapter () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DismissAnimatorListenerAdapter.class)
			mono.android.TypeManager.Activate ("XpressCode.DismissAnimatorListenerAdapter, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public DismissAnimatorListenerAdapter (xpresscode.SwipeDismissListViewTouchListener p0, int p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == DismissAnimatorListenerAdapter.class)
			mono.android.TypeManager.Activate ("XpressCode.DismissAnimatorListenerAdapter, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "XpressCode.SwipeDismissListViewTouchListener, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1 });
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
