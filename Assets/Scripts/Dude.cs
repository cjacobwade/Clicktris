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

	[SerializeField]
	float _maxEnlargeSize = 2f;

	[SerializeField]
	int _maxEnlargeNum = 2;

	[HideInInspector]
	public int numEnlarges = 0;

	[SerializeField]
	float _enlargeTime = 0.2f;

	[SerializeField]
	AnimationCurve _enlargeCurve = new AnimationCurve();

	Coroutine _enlargeRoutine = null;

	ParticleSystem _heartVFX = null;

	Coroutine _goToTargetRoutine = null;

	GroundShadow _shadow = null;

	Vector3 _prevPos = Vector3.zero;

	[SerializeField, Range(-1f, 1f)]
	float _cullDot = 0.1f;

	void Start()
	{
		_heartVFX = GetComponentInChildren<ParticleSystem>();
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		_animator = GetComponentInChildren<Animator>();
		_shadow = GetComponent<GroundShadow>();

		_clickTween = GetComponent<ScaleTween>();

		animator.Play("DudeIdle", 0, Random.value);

		transform.position = Planet.GetNearestSurfacePos(transform.position);
		transform.position += Planet.GetNormalAtPosition(transform.position) * _groundOffset;
	}

	void Update()
	{
		if (_goToTargetRoutine == null && Time.time - _lastClickTime > _clickWaitTime)
			SetTargetPos(transform.position + Random.insideUnitSphere * _wanderDist);

		Vector3 toDude = (transform.position - Planet.instance.transform.position).normalized;
		Vector3 toCamera = (UIManager.GetPanel<InventoryPanel>().GetViewRect().position - Planet.instance.transform.position).normalized;
		float edgeDot = Vector3.Dot(toDude, toCamera);
		_spriteRenderer.enabled = edgeDot > _cullDot;
		_shadow.GetShadow().enabled = edgeDot > _cullDot;
	}

	void SetTargetPos(Vector3 targetPos)
	{
		_targetPos = Planet.GetNearestSurfacePos(targetPos);

		if (_goToTargetRoutine != null)
			StopCoroutine(_goToTargetRoutine);

		_goToTargetRoutine = StartCoroutine(GoToTargetRoutine());
	}

	public bool CanEnlarge()
	{ return numEnlarges < _maxEnlargeNum; }

	public void Enlarge()
	{
		numEnlarges++;

		if (_enlargeRoutine != null)
			StopCoroutine(_enlargeRoutine);

		_enlargeRoutine = StartCoroutine(EnlargeRoutine());
	}

	void OnMouseDown()
	{
		_heartVFX.Emit(CameraOrbit.GetClickStrength());

		_numClicks += CameraOrbit.GetClickStrength();
		if (_numClicks > _clicksToDrop + numEnlarges * _clicksToDrop * 0.75f)
		{
			_heartVFX.Emit(10);

			for (int i = 0; i <= numEnlarges; i++)
			{
				Block block = Instantiate<Block>(_blockPrefab, Planet.instance.transform);
				StartCoroutine(DropBlockRoutine(block));
			}

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

	IEnumerator EnlargeRoutine()
	{
		_lastClickTime = Time.time;
		if (_goToTargetRoutine != null)
		{
			StopCoroutine(_goToTargetRoutine);
			_goToTargetRoutine = null;
		}

		_clickTween.enabled = false;
		animator.enabled = false;

		Vector3 startScale = transform.localScale;
		Vector3 endScale = Vector3.Lerp(Vector3.one, Vector3.one * _maxEnlargeSize, numEnlarges / (float)_maxEnlargeNum);

		float timer = 0f;
		while(timer < _enlargeTime)
		{
			timer += Time.deltaTime;

			float alpha = _enlargeCurve.Evaluate(timer / _enlargeTime);
			transform.localScale = Vector3.LerpUnclamped(startScale, endScale, alpha);
			_clickTween.initScale = transform.localScale;

			yield return null;
		}

		_clickTween.enabled = true;
		animator.enabled = true;

		_enlargeRoutine = null;
	}

	IEnumerator DropBlockRoutine(Block block)
	{
		for(int i = 0; i < block.GetChildColliders().Length; i++)
			block.GetChildColliders()[i].enabled = false;

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

		for (int i = 0; i < block.GetChildColliders().Length; i++)
			block.GetChildColliders()[i].enabled = true;
	}

	IEnumerator GoToTargetRoutine()
	{
		_prevPos = transform.position;

		float targetDist = Vector3.Distance(Planet.GetNearestSurfacePos(transform.position), _targetPos);
		while(targetDist > _maxReachTargetDist)
		{
			Vector3 normal = Planet.GetNormalAtPosition(transform.position);
			Vector3 toTarget = _targetPos - transform.position;
			Vector3 right = Vector3.Cross(normal, toTarget.normalized);
			Vector3 tangent = Vector3.Cross(right, normal);

			// Need to subtract out movement caused by camera orbit
			Vector2 camScreenDelta = Camera.main.WorldToScreenPoint(transform.position) - Camera.main.WorldToScreenPoint(_prevPos);

			transform.position += tangent * Mathf.Min(toTarget.magnitude, _moveSpeed * Time.deltaTime);
			transform.position = Planet.GetNearestSurfacePos(transform.position) + Planet.GetNormalAtPosition(transform.position) * _groundOffset;

			Vector2 screenDelta = Camera.main.WorldToScreenPoint(transform.position) - Camera.main.WorldToScreenPoint(_prevPos);
			screenDelta -= camScreenDelta;

			spriteRenderer.flipX = screenDelta.x > 0f;
			_prevPos = transform.position;

			yield return null;

			targetDist = Vector3.Distance(Planet.GetNearestSurfacePos(transform.position), _targetPos);
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