using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using XpressCode;


namespace SwipeDismissListView
{
	[Activity (Label = "SwipeDismissListView", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity,IDismissCallbacks,IUndoListener
	{
		UndoBarController mUndoBarController;
		ArrayAdapter<String> mAdapter;
		ListView list;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			String[] items = new String[20];
			for (int i = 0; i < items.Length; i++) {
				items[i] = "Item " + (i + 1);
			}

			list = FindViewById<ListView> (Resource.Id.lstItems);

			mAdapter = new ArrayAdapter<String>(this,Android.Resource.Layout.SimpleListItem1,Android.Resource.Id.Text1,new List<String>(items));
			list.Adapter=mAdapter;

			SwipeDismissListViewTouchListener touchListener = new SwipeDismissListViewTouchListener(list,this);
		
			list.SetOnTouchListener (touchListener);
			list.SetOnScrollListener (touchListener.MakeScrollListener());

			mUndoBarController = new UndoBarController(FindViewById(Resource.Id.undobar), this);
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);
			mUndoBarController.onSaveInstanceState(outState);
		}

		protected override void OnRestoreInstanceState (Bundle savedInstanceState)
		{
			base.OnRestoreInstanceState (savedInstanceState);
			mUndoBarController.onRestoreInstanceState(savedInstanceState);
		}

		public bool canDismiss (int position)
		{
			return true;
		}
		int _position;	
		public void onDismiss (ListView listView, int[] reverseSortedPositions)
		{
			foreach (int position in reverseSortedPositions) {
				var item = mAdapter.GetItem (position);
				mAdapter.Remove(item);
				_position = position;
				mUndoBarController.ShowUndoBar(false,GetString(Resource.String.undobar_sample_message),item);
			}
			mAdapter.NotifyDataSetChanged();
		}

		public void OnUndo(object token) {
			mAdapter.Insert ((string)token, _position);
			// Perform the undo
		}
	}
}


