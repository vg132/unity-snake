using System;

namespace Assets.VGSoftware.Scripts.Grid
{
	public class OnGridValueChangedEventArgs<TGridObject> : EventArgs
	{
		public int X { get; private set; }
		public int Y { get; private set; }
		public TGridObject Value { get; private set; }

		public OnGridValueChangedEventArgs(int x, int y, TGridObject value)
		{
			X = x;
			Y = y;
			Value = value;
		}
	}
}