using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : WadeBehaviour
{
	Animator[] _blockAnims = null;
	SpriteRenderer[] _bitSprites = null;

	LensHandle<bool> _activeInputLens = null;

	Vector3 _pickupPos = Vector3.zero;

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

	bool _prevUIOverlap = false;

	Dictionary<SpriteRenderer, BitSlotWidget> _spriteToSlotMap = new Dictionary<SpriteRenderer, BitSlotWidget>();

	void Awake()
	{
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

			for(int i = 0; i < _bitSprites.Length; i++)
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
		if(CameraOrbit.inputFreeLens && _dropRoutine == null)
		{
			_pickupPos = transform.position;
			_prevUIOverlap = false;

			for (int i = 0; i < _bitSprites.Length; i++)
				_bitSprites[i].sortingOrder = _uiLayerOrder;

			_activeInputLens = new LensHandle<bool>(this, false);

			CameraOrbit.unlockedLens.AddRequest(new LensHandle<bool>(this, false));
			CameraOrbit.inputFreeLens.AddRequest(_activeInputLens);
		}
	}

	void OnMouseDrag()
	{
		if (_activeInputLens != null)
		{
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = worldPos + Camera.main.transform.forward * _selectedCamOffset;

			Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
			RectTransform inventoryRect = UIManager.GetPanel<InventoryPanel>().GetInventoryRect();

			bool uiOverlap = RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, screenPos, Camera.main);
			if(uiOverlap != _prevUIOverlap)
			{
				if (_changeScaleRoutine != null)
					StopCoroutine(_changeScaleRoutine);

				_changeScaleRoutine = StartCoroutine(ChangeScaleRoutine(uiOverlap));
				_prevUIOverlap = uiOverlap;
			}

			_spriteToSlotMap.Clear();
			BitSlotWidget[] bitSlots = UIManager.GetPanel<InventoryPanel>().GetBitSlots();
			for (int i = 0; i < bitSlots.Length; i++)
				bitSlots[i].SetHighlight(false);

			for(int i = 0; i < _bitSprites.Length; i++)
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
			foreach(var kvp in _spriteToSlotMap)
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
				foreach(var kvp in _spriteToSlotMap)
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
				if (_changeScaleRoutine != null)
					StopCoroutine(_changeScaleRoutine);

				_changeScaleRoutine = StartCoroutine(ChangeScaleRoutine(false));
				_dropRoutine = StartCoroutine(DropRoutine());
			}
		}
	}

	IEnumerator DropRoutine()
	{
		Vector3 startPos = transform.position;
		Vector3 endPos = _pickupPos;

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
