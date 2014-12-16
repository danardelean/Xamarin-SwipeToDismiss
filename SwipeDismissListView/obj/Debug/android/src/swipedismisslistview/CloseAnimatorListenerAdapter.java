package swipedismisslistview;


public class CloseAnimatorListenerAdapter
	extends android.animation.AnimatorListenerAdapter
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onAnimationEnd:(Landroid/animation/Animator;)V:GetOnAnimationEnd_Landroid_animation_Animator_Handler\n" +
			"";
		mono.android.Runtime.register ("SwipeDismissListView.CloseAnimatorListenerAdapter, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CloseAnimatorListenerAdapter.class, __md_methods);
	}


	public CloseAnimatorListenerAdapter () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CloseAnimatorListenerAdapter.class)
			mono.android.TypeManager.Activate ("SwipeDismissListView.CloseAnimatorListenerAdapter, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
