using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreManager : Singleton<ScoreManager>
{
	public Text ScoreText;
	public Text ComboText;
	public Text DistanceText;
	public CanvasGroup ComboGroup;
	public RectTransform ComboContainer;
	public int ScorePerNode = 100;
	public int MaxCombo = 10;
	
	public int MaxDistance { get; private set; }

	public int Score => _score;

	private int _sequenceCombo = 0;
	private int _score = 0;
	private int _lastIndexHit = 0;
	private Tween _comboTween;
	private bool _comboShown = false;
	private float _playerStartY;
	
	public void HitNode(int index)
	{
		if(index == _lastIndexHit + 1)
		{
			_sequenceCombo++;
		}
		else
		{
			_sequenceCombo = 0;
		}

		_score += ScorePerNode;
		if(_sequenceCombo > 1)
		{
			var comboNumber = Mathf.Min(MaxCombo, _sequenceCombo) - 1;
			_score += (int)Mathf.Pow(1.5f, comboNumber) * ScorePerNode;
		}

		_lastIndexHit = index;
	}

	private void Awake()
	{
		_playerStartY = Player.Instance.transform.position.y;
		ComboGroup.alpha = 0;
	}

	private void Update()
	{
		ScoreText.text = _score.ToString().PadLeft(6, '0');
		ComboText.text = $"SEQUENCE COMBO\nx{_sequenceCombo}";
		var playerDistance = Mathf.FloorToInt(Player.Instance.transform.position.y - _playerStartY);
		DistanceText.text = $"{playerDistance * 100}m";
		MaxDistance = Mathf.Max(playerDistance * 100, MaxDistance);

		if(_sequenceCombo > 1 && !_comboShown)
		{
			ComboContainer.gameObject.SetActive(true);
			_comboTween?.Kill();
			_comboTween = DOTween.To(
				() => ComboGroup.alpha,
				f => ComboGroup.alpha = f,
				1.0f,
				0.2f);
			_comboTween.SetEase(Ease.InQuad);
			_comboShown = true;
		}
		else if(_sequenceCombo < 2 && _comboShown)
		{
			_comboTween?.Kill();
			_comboTween = DOTween.To(
				() => ComboGroup.alpha,
				f => ComboGroup.alpha = f,
				0.0f,
				0.2f);
			_comboTween.SetEase(Ease.InQuad);
			_comboShown = false;
		}
		
		RectTransformUtility.ScreenPointToWorldPointInRectangle(
			ComboContainer.transform.parent as RectTransform,
			Camera.main.WorldToScreenPoint(Player.Instance.transform.position),
			Camera.main,
			out var worldPoint);
		ComboContainer.transform.localPosition = worldPoint;
	}
}