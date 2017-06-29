using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScrollUVs : WadeBehaviour
{
	[SerializeField]
	string _propertyName = "_MainTex";

	[SerializeField]
	Vector2 _scrollSpeed = Vector2.zero;

	Material _mat = null;

	[SerializeField]
	bool _previewInEditor = false;

	void Awake()
	{
		if (Application.isPlaying)
			_mat = renderer.material;
		else if(_previewInEditor)
			_mat = renderer.sharedMaterial;
	}

	void Update()
	{
		if (_mat && !string.IsNullOrEmpty(_propertyName) && _mat.HasProperty(_propertyName))
			_mat.SetTextureOffset(_propertyName, _mat.GetTextureOffset(_propertyName) + _scrollSpeed * Time.deltaTime);
	}
}
