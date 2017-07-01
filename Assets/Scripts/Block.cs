using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : WadeBehaviour
{
	Animator[] _blockAnims = null;
	SpriteRenderer[] _bitSprites = null;

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

	[SerializeField]
	float _gridSlotOffset = 0.3f;

	Coroutine _changeScaleRoutine = null;

	[SerializeField]
	Transform _rotatePivot = null;

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

	Dictionary<SpriteRenderer, BitSlotWidget> _spriteToSlotMap = new Dictionary<SpriteRenderer, BitSlotWidget>();

	void Awake()
	{
		_groundShadow = GetComponent<GroundShadow>();
		_planetLayer = 1 << LayerMask.NameToLayer("Planet");

		_bitSprites = GetComponentsInChildren<SpriteRenderer>();

		_blockAnims = GetComponentsInChildren<Animator>();
		for (int i = 0; i < _blockAnims.Length; i++)
			_blockAnims[i].Play(string.Format("BlockJiggle{0}", Random.Range(1, 4)), 0, Random.value);
	}

	void Update()
	{
		if (_activeInputLens == null)
		{
			Vector3 toBlock = (transform.position - Planet.instance.transform.position).normalized;
			float edgeDot = Vector3.Dot(toBlock, Camera.main.transform.forward);

			for (int i = 0; i < _bitSprites.Length; i++)
				_bitSprites[i].enabled = edgeDot < 0.7f;
		}
		else
		{
			for (int i = 0; i < _bitSprites.Length; i++)
				_bitSprites[i].enabled = true;
		}

		for (int i = 0; i < _bitSprites.Length; i++)
			_bitSprites[i].transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up);
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
			_prevUIOverlap = RectTransformUtility.RectangleContainsScreenPoint(inventoryPanel.GetInventoryRect(), mousePos, Camera.main);
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

			InventoryPanel inventoryPanel = UIManager.GetPanel<InventoryPanel>();
			bool uiOverlap = RectTransformUtility.RectangleContainsScreenPoint(inventoryPanel.GetInventoryRect(), mousePos, Camera.main);
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

			bool leftOverlap = RectTransformUtility.RectangleContainsScreenPoint(inventoryPanel.GetLeftRotateRect(), mousePos, Camera.main);
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

			bool rightOverlap = RectTransformUtility.RectangleContainsScreenPoint(inventoryPanel.GetRightRotateRect(), mousePos, Camera.main);
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
			BitSlotWidget[] bitSlots = UIManager.GetPanel<InventoryPanel>().GetBitSlots();
			for (int i = 0; i < bitSlots.Length; i++)
				bitSlots[i].SetHighlight(false);

			for (int i = 0; i < _bitSprites.Length; i++)
			{
				SpriteRenderer bitSprite = _bitSprites[i];
				Vector3 spriteViewPos = Camera.main.WorldToViewportPoint(bitSprite.transform.position);
				for (int j = 0; j < bitSlots.Length; j++)
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
		}
	}

	void OnMouseUp()
	{
		if (_activeInputLens != null)
		{
			CameraOrbit.unlockedLens.RemoveRequestsWithContext(this);
			CameraOrbit.inputFreeLens.RemoveRequestsWithContext(this);
			_activeInputLens = null;

			BitSlotWidget[] bitSlots = UIManager.GetPanel<InventoryPanel>().GetBitSlots();
			for (int i = 0; i < bitSlots.Length; i++)
				bitSlots[i].SetHighlight(false);

			bool allMatched = _spriteToSlotMap.Count == _bitSprites.Length;
			if (allMatched)
			{
				foreach (var kvp in _spriteToSlotMap)
				{
					BitSlotWidget slot = kvp.Value;
					slot.used = true;

					SpriteRenderer sprite = kvp.Key;
					GameObject spriteParent = new GameObject("SpriteHolder");
					sprite.transform.SetParent(spriteParent.transform);
					sprite.transform.localPosition = Vector3.zero;

					spriteParent.transform.SetParent(Camera.main.transform);
					spriteParent.transform.localScale = transform.localScale;
					spriteParent.transform.position = slot.transform.position + -Camera.main.transform.forward * _gridSlotOffset;
				}

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

				InventoryPanel inventoryPanel = UIManager.GetPanel<InventoryPanel>();
				bool uiOverlap = RectTransformUtility.RectangleContainsScreenPoint(inventoryPanel.GetInventoryRect(), mousePos, Camera.main);
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
		}
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
