using UnityEngine;

public class PlayerController : CachedMonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private NavMeshAgent _navMeshAgent;
	[SerializeField] private ParticleSystem _rocksFX;
	[SerializeField] private ParticleSystem _dustFX;

	private EnemyController _target;
	private bool _isAttacking;

	private void Update()
	{
		if (_target != null)
		{
			if (!_isAttacking)
			{
				float distance = Vector3.Distance(CachedTransform.position, _target.CachedTransform.position);
				if (distance < 3f)
				{
					_isAttacking = true;
					_animator.SetTrigger("Attack");
					_navMeshAgent.Stop();
					Invoke("KillTarget", 0.25f);
				}
			}
		}

		_animator.SetFloat("LocomotionSpeed", _navMeshAgent.velocity.magnitude);

		if (_navMeshAgent.velocity.magnitude > 0)
		{
			_dustFX.Emit(2);
		}
	}

	private void KillTarget()
	{
		_target.OnKilled();
		_rocksFX.Emit(15);
		_isAttacking = false;
		_target = null;
	}

	public void GoToPosition(Vector3 position)
	{
		if (!_isAttacking)
		{
			_target = null;
			_navMeshAgent.SetDestination(position);
		}
	}

	public void GoToEnemy(EnemyController enemy)
	{
		if (!_isAttacking)
		{
			_target = enemy;
			_navMeshAgent.SetDestination(enemy.CachedTransform.position);
		}
	}

	public void Die()
	{
		_animator.SetTrigger("Die");
		_navMeshAgent.Stop();
	}
}
