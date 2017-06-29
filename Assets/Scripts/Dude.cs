using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude : WadeBehaviour
{
	Projector _projector = null;

	ScaleTween _clickTween = null;

	[SerializeField]
	float _projectorOffset = 0.3f;

	Vector3 _relativeTargetPos = Vector3.zero;

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

	Coroutine _goToTargetRoutine = null;

	Vector3 _prevPos = Vector3.zero;

	void Start()
	{
		_projector = GetComponentInChildren<Projector>();
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		_animator = GetComponentInChildren<Animator>();

		_clickTween = GetComponent<ScaleTween>();

		animator.Play("DudeIdle", 0, Random.value);

		transform.position = Planet.GetNearestSurfacePos(transform.position);
		transform.position += Planet.GetNormalAtPosition(transform.position) * _groundOffset;
	}

	void FixedUpdate()
	{
		_projector.transform.position = transform.position + Planet.GetNormalAtPosition(transform.position) * _projectorOffset;
		_projector.transform.LookAt(transform.position);

		if (_goToTargetRoutine == null && Time.time - _lastClickTime > _clickWaitTime)
			SetTargetPos(transform.position + Random.insideUnitSphere * _wanderDist);
	}

	void SetTargetPos(Vector3 targetPos)
	{
		_relativeTargetPos = Planet.GetNearestSurfacePos(targetPos);
		_relativeTargetPos = Planet.instance.transform.InverseTransformPoint(_relativeTargetPos);

		if (_goToTargetRoutine != null)
			StopCoroutine(_goToTargetRoutine);

		_goToTargetRoutine = StartCoroutine(GoToTargetRoutine());
	}

	void OnMouseDown()
	{
		_lastClickTime = Time.time;
		_clickTween.Play();

		if (_goToTargetRoutine != null)
		{
			StopCoroutine(_goToTargetRoutine);
			_goToTargetRoutine = null;
		}
	}

	IEnumerator GoToTargetRoutine()
	{
		_prevPos = transform.position;

		Vector3 worldTargetPos = Planet.instance.transform.TransformPoint(_relativeTargetPos);
		while(Vector3.Distance(transform.position, worldTargetPos) > _maxReachTargetDist)
		{
			Vector3 currentNormal = Planet.GetNormalAtPosition(transform.position);
			Vector3 targetNormal = Planet.GetNormalAtPosition(worldTargetPos);

			Vector3 axis = transform.InverseTransformDirection(Vector3.Cross(currentNormal, targetNormal));
			float angle = Vector3.Angle(currentNormal, targetNormal);

			// Need to subtract out movement caused by camera orbit
			Vector2 camScreenDelta = Camera.main.WorldToScreenPoint(transform.position) - Camera.main.WorldToScreenPoint(_prevPos);

			transform.RotateAround(Planet.instance.transform.position, axis, Mathf.Min(angle, _moveSpeed * Time.deltaTime));

			Vector2 screenDelta = Camera.main.WorldToScreenPoint(transform.position) - Camera.main.WorldToScreenPoint(_prevPos);
			screenDelta -= camScreenDelta;

			spriteRenderer.flipX = screenDelta.x < 0f;

			_prevPos = transform.position;

			yield return new WaitForFixedUpdate();

			worldTargetPos = Planet.instance.transform.TransformPoint(_relativeTargetPos);
		}

		_goToTargetRoutine = null;
	}

	void OnDrawGizmosSelected()
	{
		Vector3 worldTargetPos = Planet.instance.transform.TransformPoint(_relativeTargetPos);

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(worldTargetPos, 0.2f);

		int pathIterations = 50;
		for (int i = 1; i < pathIterations; i++)
		{
			Vector3 prevPos = Vector3.Lerp(transform.position, worldTargetPos, (i - 1) / (float)pathIterations);
			prevPos = Planet.GetNearestSurfacePos(prevPos);

			Vector3 nextPos = Vector3.Lerp(transform.position, worldTargetPos, i / (float)pathIterations);
			nextPos = Planet.GetNearestSurfacePos(nextPos);

			Gizmos.color = Color.white;
			Gizmos.DrawLine(prevPos, nextPos);
		}
	}
}