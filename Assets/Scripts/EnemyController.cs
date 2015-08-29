using UnityEngine;
using Random = UnityEngine.Random;

using System;
using System.Collections;

public class EnemyController : CachedMonoBehaviour
{
	[SerializeField] private GameObject _detectFX;
	[SerializeField] private GameObject _shotFX;
	[SerializeField] private GameObject _shadowPlaneGO;
	[SerializeField] private Animator _animator;

	public event EventHandler OnEnemyKilled;
	public event EventHandler OnCorpseDetected;
	public event EventHandler OnPlayerDetected;

	private readonly int[] _targetAngles = { 90, 0, -90, 180 };

	private const float _timeToChange = 2f;
	private int _currentAngle = 0;

	public bool IsDead { get; private set; }

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(Random.Range(0f, 2f));
		while (true)
		{
			transform.eulerAngles = new Vector3(0f, _targetAngles[_currentAngle], 0f);
			yield return new WaitForSeconds(_timeToChange);
			_currentAngle++;
			if (_currentAngle >= _targetAngles.Length)
			{
				_currentAngle = 0;
			}
		}
	}

	private void OnMouseUpAsButton()
	{
		AppManager.GameController.Player.GoToEnemy(this);
	}

	private void Update()
	{
		// Check player in range
		if (TargetInRange(AppManager.GameController.Player.CachedTransform.position))
		{
			_shotFX.SetActive(true);
			if (OnPlayerDetected != null)
			{
				OnPlayerDetected(this, EventArgs.Empty);
			}
		}

		// Check corpse in range
		foreach (EnemyController enemy in AppManager.GameController.Enemies)
		{
			if (enemy.IsDead)
			{
				if (TargetInRange(enemy.CachedTransform.position))
				{
					if (OnCorpseDetected != null)
					{
						OnCorpseDetected(this, EventArgs.Empty);
					}
				}
			}
		}
	}

	public void OnKilled()
	{
		_animator.SetTrigger("Die");
		IsDead = true;
		_shadowPlaneGO.SetActive(false);
		if (OnEnemyKilled != null)
		{
			OnEnemyKilled(this, EventArgs.Empty);
		}
		OnEndGame();
	}

	private bool TargetInRange(Vector3 targetPos)
	{
		Vector3 toPlayerVector = targetPos - CachedTransform.position;
		return (toPlayerVector.magnitude < 5f && Vector3.Angle(toPlayerVector, CachedTransform.forward) < 15f);
	}

	public void OnEndGame()
	{
		enabled = false;
		StopAllCoroutines();
		Collider.enabled = false;
		_detectFX.SetActive(false);
	}
}
