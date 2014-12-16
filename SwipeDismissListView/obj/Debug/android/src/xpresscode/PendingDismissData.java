package xpresscode;


public class PendingDismissData
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("XpressCode.PendingDismissData, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PendingDismissData.class, __md_methods);
	}


	public PendingDismissData () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PendingDismissData.class)
			mono.android.TypeManager.Activate ("XpressCode.PendingDismissData, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public PendingDismissData (int p0, android.view.View p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == PendingDismissData.class)
			mono.android.TypeManager.Activate ("XpressCode.PendingDismissData, SwipeDismissListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}

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
