using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude : WadeBehaviour
{
	ScaleTween _clickTween = null;

	Vector3 _targetPos = Vector3.zero;

	[SerializeField]
	float _maxReachTargetDist = 0.3f;

	[SerializeField]
	float _moveSpeed = 7f;

	[SerializeField]
	float _groundOffset = 0.1f;

	[SerializeField]
	float _wanderDist = 8f;

	[SerializeField]
	float _clickWaitTime = 0.7f;

	float _lastClickTime = -10000f;

	[SerializeField]
	int _clicksToDrop = 5;

	int _numClicks = 0;

	[SerializeField]
	Block _blockPrefab = null;

	[SerializeField]
	float _blockDropTime = 1f;

	[SerializeField]
	float _blockRandomRange = 1f;

	[SerializeField]
	float _blockArcHeight = 1f;

	[SerializeField]
	float _blockGroundOffset = 0.15f;

	Coroutine _goToTargetRoutine = null;

	Vector3 _prevPos = Vector3.zero;

	void Start()
	{
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		_animator = GetComponentInChildren<Animator>();

		_clickTween = GetComponent<ScaleTween>();

		animator.Play("DudeIdle", 0, Random.value);

		transform.position = Planet.GetNearestSurfacePos(transform.position);
		transform.position += Planet.GetNormalAtPosition(transform.position) * _groundOffset;
	}

	void Update()
	{
		if (_goToTargetRoutine == null && Time.time - _lastClickTime > _clickWaitTime)
			SetTargetPos(transform.position + Random.insideUnitSphere * _wanderDist);
	}

	void SetTargetPos(Vector3 targetPos)
	{
		_targetPos = Planet.GetNearestSurfacePos(targetPos);

		if (_goToTargetRoutine != null)
			StopCoroutine(_goToTargetRoutine);

		_goToTargetRoutine = StartCoroutine(GoToTargetRoutine());
	}

	void OnMouseDown()
	{
		if (++_numClicks > _clicksToDrop)
		{
			Block block = Instantiate<Block>(_blockPrefab, Planet.instance.transform);
			StartCoroutine(DropBlockRoutine(block));

			_numClicks = 0;
		}

		_lastClickTime = Time.time;
		_clickTween.Play();

		if (_goToTargetRoutine != null)
		{
			StopCoroutine(_goToTargetRoutine);
			_goToTargetRoutine = null;
		}
	}

	IEnumerator DropBlockRoutine(Block block)
	{
		Vector3 startPos = transform.position;

		Vector3 randomOffset = Random.insideUnitSphere;
		Vector3 normal = Planet.GetNormalAtPosition(transform.position);
		randomOffset -= normal * Mathf.Abs(Vector3.Dot(randomOffset.normalized, normal));

		Vector3 endPos = transform.position + randomOffset.normalized * _blockRandomRange;
		endPos = Planet.GetNearestSurfacePos(endPos);
		endPos += Planet.GetNormalAtPosition(endPos) * _blockGroundOffset;

		float timer = 0f;
		while(timer < _blockDropTime)
		{
			timer += Time.deltaTime;

			float alpha = timer / _blockDropTime;
			block.transform.position = Vector3.Lerp(startPos, endPos, alpha);
			block.transform.position += Planet.GetNormalAtPosition(block.transform.position) * Mathf.Sin(alpha * Mathf.PI) * _blockArcHeight;

			yield return null;
		}
	}

	IEnumerator GoToTargetRoutine()
	{
		_prevPos = transform.position;

		while(Vector3.Distance(transform.position, _targetPos) > _maxReachTargetDist)
		{
			Vector3 currentNormal = Planet.GetNormalAtPosition(transform.position);
			Vector3 targetNormal = Planet.GetNormalAtPosition(_targetPos);

			Vector3 axis = transform.InverseTransformDirection(Vector3.Cross(currentNormal, targetNormal));
			float angle = Vector3.Angle(currentNormal, targetNormal);

			// Need to subtract out movement caused by camera orbit
			Vector2 camScreenDelta = Camera.main.WorldToScreenPoint(transform.position) - Camera.main.WorldToScreenPoint(_prevPos);

			transform.RotateAround(Planet.instance.transform.position, axis, Mathf.Min(angle, _moveSpeed * Time.deltaTime));

			Vector2 screenDelta = Camera.main.WorldToScreenPoint(transform.position) - Camera.main.WorldToScreenPoint(_prevPos);
			screenDelta -= camScreenDelta;

			spriteRenderer.flipX = screenDelta.x > 0f;

			_prevPos = transform.position;

			yield return null;
		}

		_goToTargetRoutine = null;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(_targetPos, 0.2f);

		int pathIterations = 50;
		for (int i = 1; i < pathIterations; i++)
		{
			Vector3 prevPos = Vector3.Lerp(transform.position, _targetPos, (i - 1) / (float)pathIterations);
			prevPos = Planet.GetNearestSurfacePos(prevPos);

			Vector3 nextPos = Vector3.Lerp(transform.position, _targetPos, i / (float)pathIterations);
			nextPos = Planet.GetNearestSurfacePos(nextPos);

			Gizmos.color = Color.white;
			Gizmos.DrawLine(prevPos, nextPos);
		}
	}
}