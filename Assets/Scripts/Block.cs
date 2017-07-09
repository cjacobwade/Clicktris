using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
	Bun,
	Red,
	Gat,
	Bop,
	Bull,
	Random
}

public class Block : WadeBehaviour
{
	InventoryPanel _inventoryPanel = null;
	InventoryPanel GetInventory()
	{
		if (!_inventoryPanel)
			_inventoryPanel = UIManager.GetPanel<InventoryPanel>();

		return _inventoryPanel;
	}

	[SerializeField]
	BlockType _blockType = BlockType.Bun;
	public BlockType GetBlockType()
	{ return _blockType; }

	[SerializeField]
	Sprite _defaultSprite = null;

	[SerializeField]
	Sprite _goldSprite = null;

	bool _gold = false;

	Animator[] _blockAnims = null;
	SpriteRenderer[] _bitSprites = null;
	public SpriteRenderer[] GetBitSprites()
	{ return _bitSprites; }

	LensHandle<bool> _activeInputLens = null;

	Vector3 _dropPos = Vector3.zero;

	[SerializeField]
	float _dropTime = 0.2f;

	[SerializeField]
	float _dropArcHeight = 2f;

	[SerializeField]
	float _selectedCamOffset = 0.5f;

	[SerializeField]
	int _worldLayerOrder = 0;

	[SerializeField]
	int _uiLayerOrder = 1;

	Coroutine _dropRoutine = null;

	[SerializeField]
	float _uiScale = 2.7f;

	[SerializeField]
	float _changeScaleTime = 0.5f;

	[SerializeField]
	float _gridSlotMaxDist = 0.1f;

	Coroutine _changeScaleRoutine = null;

	[SerializeField]
	Transform _rotatePivot = null;
	public Transform GetRotatePivot()
	{ return _rotatePivot; }

	[SerializeField]
	float _rotateTime = 0.2f;

	float _targetZRotation = 0f;

	Coroutine _rotateRoutine = null;

	[SerializeField]
	float _dropGroundOffset = 0.15f;

	bool _prevUIOverlap = false;
	bool _prevLeftOverlap = false;
	bool _prevRightOverlap = false;

	GroundShadow _groundShadow = null;
	LayerMask _planetLayer = 0;
	RaycastHit _hitInfo = new RaycastHit();

	[SerializeField]
	float _timeBeforeFadeout = 5f;

	[SerializeField]
	float _fadeoutTime = 3f;

	[SerializeField]
	AnimationCurve _fadeoutCurve = new AnimationCurve();

	[SerializeField]
	int _numFadeoutFlashes = 4;

	Coroutine _fadeoutRoutine = null;

	float _lastTouchTime = 0f;

	[SerializeField, Range(-1f, 1f)]
	float _cullDot = 0.1f;

	Collider[] _childColliders = null;
	public Collider[] GetChildColliders()
	{ return _childColliders; }

	Dictionary<SpriteRenderer, BitSlotWidget> _spriteToSlotMap = new Dictionary<SpriteRenderer, BitSlotWidget>();
	public Dictionary<SpriteRenderer, BitSlotWidget> GetSpriteToSlotMap()
	{ return _spriteToSlotMap; }

	void Awake()
	{
		_childColliders = GetComponentsInChildren<Collider>();

		_groundShadow = GetComponent<GroundShadow>();
		_planetLayer = 1 << LayerMask.NameToLayer("Planet");

		_bitSprites = GetComponentsInChildren<SpriteRenderer>();

		_blockAnims = GetComponentsInChildren<Animator>();
		for (int i = 0; i < _blockAnims.Length; i++)
			_blockAnims[i].Play(string.Format("BlockJiggle{0}", Random.Range(1, 4)), 0, Random.value);

		ResetFadeoutTimer();
	}

	void Update()
	{
		if (_activeInputLens == null)
		{
			Vector3 toBlock = (transform.position - Planet.instance.transform.position).normalized;
			Vector3 toCamera = (GetInventory().GetViewRect().transform.position - Planet.instance.transform.position).normalized;
			float edgeDot = Vector3.Dot(toBlock, toCamera);

			for (int i = 0; i < _bitSprites.Length; i++)
				_bitSprites[i].enabled = edgeDot > _cullDot;
		}
		else
		{
			for (int i = 0; i < _bitSprites.Length; i++)
				_bitSprites[i].enabled = true;
		}

		for (int i = 0; i < _bitSprites.Length; i++)
			_bitSprites[i].transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up);

		if(Time.time - _lastTouchTime > _timeBeforeFadeout && _fadeoutRoutine == null && !_gold)
			_fadeoutRoutine = StartCoroutine(FadeoutRoutine());
	}

	void OnMouseDown()
	{
		if (CameraOrbit.inputFreeLens && _dropRoutine == null)
		{
			_dropPos = transform.position;
			_groundShadow.enabled = false;

			for (int i = 0; i < _bitSprites.Length; i++)
				_bitSprites[i].sortingOrder = _uiLayerOrder;

			_activeInputLens = new LensHandle<bool>(this, false);

			CameraOrbit.unlockedLens.AddRequest(new LensHandle<bool>(this, false));
			CameraOrbit.inputFreeLens.AddRequest(_activeInputLens);

			Vector2 screenMin = Vector2.zero;
			Vector2 screenMax = new Vector2(Screen.width, Screen.height);
			screenMin += (screenMax - screenMin) * 0.001f;
			screenMax -= (screenMax - screenMin) * 0.001f;
			Vector3 mousePos = ((Vector3)WadeUtils.Clamp((Vector2)Input.mousePosition, screenMin, screenMax)).SetZ(Input.mousePosition.z);

			InventoryPanel inventoryPanel = UIManager.GetPanel<InventoryPanel>();
			inventoryPanel.GetLeftRotateRect().gameObject.SetActive(true);
			inventoryPanel.GetRightRotateRect().gameObject.SetActive(true);

			_prevUIOverlap = RectTransformUtility.RectangleContainsScreenPoint(inventoryPanel.GetInventoryRect(), mousePos, Camera.main);

			ResetFadeoutTimer();
		}
	}

	void OnMouseDrag()
	{
		if (_activeInputLens != null)
		{
			Vector2 screenMin = Vector2.zero;
			Vector2 screenMax = new Vector2(Screen.width, Screen.height);

			Vector3 mousePos = ((Vector3)WadeUtils.Clamp((Vector2)Input.mousePosition, screenMin, screenMax)).SetZ(Input.mousePosition.z);
			
			Vector2 screenBoundDiff = screenMax - screenMin;
			screenMin += screenBoundDiff * 0.001f;
			screenMax -= screenBoundDiff * 0.001f;
			mousePos = ((Vector3)WadeUtils.Clamp((Vector2)mousePos, screenMin, screenMax)).SetZ(mousePos.z);

			Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
			Vector3 rotatePivotOffset = _rotatePivot.position - transform.position;
			transform.position = worldPos + Camera.main.transform.forward * _selectedCamOffset - rotatePivotOffset;

			bool uiOverlap = RectTransformUtility.RectangleContainsScreenPoint(GetInventory().GetInventoryRect(), mousePos, Camera.main);
			if (uiOverlap != _prevUIOverlap)
			{
				if (_changeScaleRoutine != null)
					StopCoroutine(_changeScaleRoutine);

				_changeScaleRoutine = StartCoroutine(ChangeScaleRoutine(uiOverlap));

				_groundShadow.GetShadow().gameObject.SetActive(!uiOverlap);
				_prevUIOverlap = uiOverlap;
			}

			if (!uiOverlap)
			{
				if (Physics.Raycast(worldPos, Camera.main.transform.forward - Camera.main.transform.up * 0.17f, out _hitInfo, Mathf.Infinity, _planetLayer, QueryTriggerInteraction.Ignore))
					_groundShadow.SetPos(_hitInfo.point);
			}

			bool leftOverlap = RectTransformUtility.RectangleContainsScreenPoint(GetInventory().GetLeftRotateRect(), mousePos, Camera.main);
			if (leftOverlap != _prevLeftOverlap)
			{
				if (leftOverlap)
				{
					if (_rotateRoutine != null)
						StopCoroutine(_rotateRoutine);

					_rotateRoutine = StartCoroutine(RotateRoutine(false));
				}

				_prevLeftOverlap = leftOverlap;
			}

			bool rightOverlap = RectTransformUtility.RectangleContainsScreenPoint(GetInventory().GetRightRotateRect(), mousePos, Camera.main);
			if (rightOverlap != _prevRightOverlap)
			{
				if (rightOverlap)
				{
					if (_rotateRoutine != null)
						StopCoroutine(_rotateRoutine);

					_rotateRoutine = StartCoroutine(RotateRoutine(true));
				}

				_prevRightOverlap = rightOverlap;
			}

			_spriteToSlotMap.Clear();
			List<BitSlotWidget> bitSlots = GetInventory().GetBitSlots();
			for (int i = 0; i < bitSlots.Count; i++)
				bitSlots[i].SetHighlight(false);

			for (int i = 0; i < _bitSprites.Length; i++)
			{
				SpriteRenderer bitSprite = _bitSprites[i];
				Vector3 spriteViewPos = Camera.main.WorldToViewportPoint(bitSprite.transform.position);
				for (int j = 0; j < bitSlots.Count; j++)
				{
					BitSlotWidget bitSlot = bitSlots[j];
					if (!bitSlot.used)
					{
						Vector3 slotViewPos = Camera.main.WorldToViewportPoint(bitSlot.transform.position);
						if (Vector2.Distance(spriteViewPos, slotViewPos) < _gridSlotMaxDist)
						{
							// Temp-mark used so slots are distinct
							bitSlot.used = true;
							bitSlot.SetHighlight(true);
							_spriteToSlotMap.Add(bitSprite, bitSlot);
							break;
						}
					}
				}
			}

			// Unmark temp-used slots
			foreach (var kvp in _spriteToSlotMap)
				kvp.Value.used = false;

			ResetFadeoutTimer();
		}
	}

	void OnMouseUp()
	{
		if (_activeInputLens != null)
		{
			CameraOrbit.unlockedLens.RemoveRequestsWithContext(this);
			CameraOrbit.inputFreeLens.RemoveRequestsWithContext(this);
			_activeInputLens = null;

			List<BitSlotWidget> bitSlots = GetInventory().GetBitSlots();
			for (int i = 0; i < bitSlots.Count; i++)
				bitSlots[i].SetHighlight(false);

			ResetFadeoutTimer();

			bool allMatched = _spriteToSlotMap.Count == _bitSprites.Length;
			if (allMatched)
			{
				GetInventory().AssignBlock(this);
				Destroy(gameObject);
			}
			else
			{
				Vector2 screenMin = Vector2.zero;
				Vector2 screenMax = new Vector2(Screen.width, Screen.height);
				Vector2 screenBoundDiff = screenMax - screenMin;
				screenMin += screenBoundDiff * 0.001f;
				screenMax -= screenBoundDiff * 0.001f;
				Vector3 mousePos = ((Vector3)WadeUtils.Clamp((Vector2)Input.mousePosition, screenMin, screenMax)).SetZ(Input.mousePosition.z);

				bool uiOverlap = RectTransformUtility.RectangleContainsScreenPoint(GetInventory().GetInventoryRect(), mousePos, Camera.main);
				if(!uiOverlap)
				{
					Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
					if (Physics.Raycast(worldPos, Camera.main.transform.forward, out _hitInfo, Mathf.Infinity, _planetLayer, QueryTriggerInteraction.Ignore))
						_dropPos = _hitInfo.point + _hitInfo.normal * _dropGroundOffset;
				}

				if (_changeScaleRoutine != null)
					StopCoroutine(_changeScaleRoutine);

				_changeScaleRoutine = StartCoroutine(ChangeScaleRoutine(false));
				_dropRoutine = StartCoroutine(DropRoutine());
			}

			InventoryPanel inventoryPanel = UIManager.GetPanel<InventoryPanel>();
			inventoryPanel.GetLeftRotateRect().gameObject.SetActive(true);
			inventoryPanel.GetRightRotateRect().gameObject.SetActive(true);
		}
	}

	public void SetGold(bool gold)
	{
		for (int i = 0; i < _bitSprites.Length; i++)
			_bitSprites[i].sprite = gold ? _goldSprite : _defaultSprite;

		ResetFadeoutTimer();

		_gold = gold;
	}

	void ResetFadeoutTimer()
	{
		_lastTouchTime = Time.time;

		if (_fadeoutRoutine != null)
		{
			StopCoroutine(_fadeoutRoutine);

			for (int i = 0; i < _bitSprites.Length; i++)
				_bitSprites[i].color = _bitSprites[i].color.SetA(1f);

			_fadeoutRoutine = null;
		}
	}

	IEnumerator FadeoutRoutine()
	{
		float timer = 0f;
		while(timer < _fadeoutTime)
		{
			timer += Time.deltaTime;

			float alpha = _fadeoutCurve.Evaluate(timer / _fadeoutTime);
			alpha = Mathf.Abs(Mathf.Cos(alpha * Mathf.PI * _numFadeoutFlashes));
			for (int i = 0; i < _bitSprites.Length; i++)
				_bitSprites[i].color = _bitSprites[i].color.SetA(alpha);

			yield return null;
		}

		_fadeoutRoutine = null;
		Destroy(gameObject);
	}

	IEnumerator RotateRoutine(bool right)
	{
		_targetZRotation += right ? 90f : -90f;

		float startRot = _targetZRotation + (right ? -90f : 90f);
		float endRot = _targetZRotation;

		float timer = 0f;
		while(timer < _rotateTime)
		{
			timer += Time.deltaTime;

			float alpha = timer / _rotateTime;
			_rotatePivot.localRotation = Quaternion.Euler(Vector3.forward * Mathf.Lerp(startRot, endRot, alpha));

			yield return null;
		}

		_rotateRoutine = null;
	}

	IEnumerator DropRoutine()
	{
		Vector3 startPos = transform.position;
		Vector3 endPos = _dropPos;

		float timer = 0f;
		while(timer < _dropTime)
		{
			timer += Time.deltaTime;

			float alpha = Mathf.Clamp01(timer / _dropTime);
			transform.position = Vector3.Lerp(startPos, endPos, alpha);
			transform.position += Camera.main.transform.up * _dropArcHeight * Mathf.Sin(alpha * Mathf.PI);

			yield return null;
		}

		for (int i = 0; i < _bitSprites.Length; i++)
			_bitSprites[i].sortingOrder = _worldLayerOrder;

		_groundShadow.GetShadow().gameObject.SetActive(true);
		_groundShadow.enabled = true;

		_dropRoutine = null;
	}

	IEnumerator ChangeScaleRoutine(bool scaleUp)
	{
		Vector3 startScale = transform.localScale;
		Vector3 endScale = scaleUp ? Vector3.one * _uiScale : Vector3.one;

		float timer = 0f;
		while(timer < _changeScaleTime)
		{
			timer += Time.deltaTime;

			float alpha = timer / _changeScaleTime;
			transform.localScale = Vector3.Lerp(startScale, endScale, alpha);

			yield return null;
		}

		_changeScaleRoutine = null;
	}
}
