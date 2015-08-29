using UnityEngine;

using System;

public class GroundClickListener : MonoBehaviour
{
	public event EventHandler<GroundClickedEventArgs> OnGroundClick;

	private void OnMouseUpAsButton()
	{
		RaycastHit hit;
		Ray ray = AppManager.GameController.MainCamera.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out hit);
		if (OnGroundClick != null)
		{
			OnGroundClick(this, new GroundClickedEventArgs(hit.point));
		}
	}
}

public class GroundClickedEventArgs : EventArgs
{
	public Vector3 Pos { get; private set; }

	public GroundClickedEventArgs(Vector3 pos)
	{
		Pos = pos;
	}
}
