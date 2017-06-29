using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class ScaleTween : WadeBehaviour
{
	[SerializeField]
	AnimationCurve _scaleCurve = new AnimationCurve();

	[SerializeField]
	FloatRange _scaleRange = new FloatRange();

	[SerializeField]
	bool _loop = false;

	[SerializeField]
	float _tweenTime = 1f;

	[SerializeField, FormerlySerializedAs("_playOnAwake")]
	bool _playOnEnable = false;

	Coroutine _playRoutine = null;
	public bool IsRunning()
	{ return _playRoutine != null; }

	[SerializeField]
	Vector3 _initScale = Vector3.one;

	void OnEnable()
	{
		if (_playOnEnable)
			_playRoutine = StartCoroutine(PlayRoutine(true));
	}

	public void SetToDefault()
	{
		transform.localScale = _initScale;
	}

	public void SetAlpha(float alpha)
	{
		transform.localScale = _initScale * _scaleRange.UnclampedLerp(alpha);
	}

	public Coroutine Play(bool forward = true)
	{
		if (_playRoutine != null)
			StopCoroutine(_playRoutine);

		_playRoutine = StartCoroutine(PlayRoutine(forward));
		return _playRoutine;
	}

	public void Stop()
	{
		if (_playRoutine != null)
			StopCoroutine(_playRoutine);
	}

	IEnumerator PlayRoutine(bool forward)
	{
		do
		{
			float tweenTimer = 0f;
			while (tweenTimer < _tweenTime)
			{
				float alpha = _scaleCurve.Evaluate(tweenTimer / _tweenTime);
				if (!forward)
					alpha = 1f - alpha;

				transform.localScale = _initScale * Mathf.Clamp(_scaleRange.UnclampedLerp(alpha), 0f, Mathf.Infinity);

				tweenTimer += Time.deltaTime;
				yield return null;
			}
		} while (_loop);

		_playRoutine = null;
	}
}
