using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : WadeBehaviour
{
	[SerializeField]
	BlockType _eater = BlockType.Random;
	public BlockType GetEater()
	{ return _eater; }

	[SerializeField]
	int _clicksToDrop = 5;

	ScaleTween _clickTween = null;

	int _numClicks = 0;

	[SerializeField, Header("Block")]
	Block _blockPrefab = null;

	[SerializeField]
	float _blockDropTime = 1f;

	[SerializeField]
	float _blockRandomRange = 1f;

	[SerializeField]
	float _blockArcHeight = 1f;

	[SerializeField]
	float _blockGroundOffset = 0.15f;

	[SerializeField, Header("Enlarge")]
	float _maxEnlargeSize = 2f;

	[SerializeField]
	int _maxEnlargeNum = 1;

	[HideInInspector]
	public int numEnlarges = 0;

	[SerializeField]
	float _enlargeTime = 0.2f;

	[SerializeField]
	AnimationCurve _enlargeCurve = new AnimationCurve();

	Coroutine _enlargeRoutine = null;
	
	void Awake()
	{
		_clickTween = GetComponent<ScaleTween>();
	}

	void OnMouseDown()
	{
		_numClicks += CameraOrbit.GetClickStrength();
		if (_numClicks > _clicksToDrop + numEnlarges * _clicksToDrop * 0.75f)
		{
			for (int i = 0; i <= numEnlarges; i++)
			{
				Block block = Instantiate<Block>(_blockPrefab, Planet.instance.transform);
				StartCoroutine(DropBlockRoutine(block));
			}

			_numClicks = 0;
		}

		_clickTween.Play();
	}

	public bool CanEnlarge()
	{ return numEnlarges < _maxEnlargeNum; }

	IEnumerator EnlargeRoutine()
	{
		_clickTween.enabled = false;
		animator.enabled = false;

		Vector3 startScale = transform.localScale;
		Vector3 endScale = Vector3.Lerp(Vector3.one, Vector3.one * _maxEnlargeSize, numEnlarges / (float)_maxEnlargeNum);

		float timer = 0f;
		while (timer < _enlargeTime)
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
		for (int i = 0; i < block.GetChildColliders().Length; i++)
			block.GetChildColliders()[i].enabled = false;

		Vector3 startPos = transform.position;

		Vector3 randomOffset = Random.insideUnitSphere;
		Vector3 normal = Planet.GetNormalAtPosition(transform.position);
		randomOffset -= normal * Mathf.Abs(Vector3.Dot(randomOffset.normalized, normal));

		Vector3 endPos = transform.position + randomOffset.normalized * _blockRandomRange;
		endPos = Planet.GetNearestSurfacePos(endPos);
		endPos += Planet.GetNormalAtPosition(endPos) * _blockGroundOffset;

		float timer = 0f;
		while (timer < _blockDropTime)
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
}
