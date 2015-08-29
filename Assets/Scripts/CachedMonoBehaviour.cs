using UnityEngine;

public abstract class CachedMonoBehaviour : MonoBehaviour
{
	private Transform _cachedTransform;
	private Collider _cachedCollider;

	public Transform CachedTransform
	{
		get { return _cachedTransform != null ? _cachedTransform : _cachedTransform = transform; }
	}

	public Collider Collider
	{
		get { return _cachedCollider != null ? _cachedCollider : _cachedCollider = collider; }
	}
}
