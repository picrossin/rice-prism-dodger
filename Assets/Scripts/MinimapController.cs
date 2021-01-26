using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class MinimapController : MonoBehaviour
{
    [SerializeField] private float minimapRadius = 120f;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string finishTag = "Finish";
    [SerializeField] private UILineRenderer _uiLineRenderer;
    
    private GameObject _playerIcon;
    private GameObject _player;
    private GameObject _finish;
    private float _scalar;
    private bool _initializedScalar;

    private void Start()
    {
        _playerIcon = Instantiate(gameObject, transform.position, Quaternion.identity);
        _playerIcon.name = "PlayerIcon";
        _playerIcon.transform.SetParent(transform.parent, false);
        Destroy(_playerIcon.GetComponent<MinimapController>());
        _playerIcon.transform.localScale *= 0.6f;
    }

    private void Update()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag(playerTag);
            return;
        }

        if (_finish == null)
        {
            _finish = GameObject.FindGameObjectWithTag(finishTag);
            return;
        }

        // Translate in-game coordinate system to minimap UI coords
        Vector2 playerCoordinate = new Vector2(_player.transform.position.x, _player.transform.position.z);
        Vector2 finishCoordinate = new Vector2(_finish.transform.position.x, _finish.transform.position.z);
        
        if (!_initializedScalar)
        {
            _scalar = minimapRadius / Vector2.Distance(playerCoordinate, finishCoordinate);
            _initializedScalar = true;
        }
        
        playerCoordinate -= finishCoordinate;
        playerCoordinate *= _scalar;
        _playerIcon.transform.position = transform.position + (Vector3) playerCoordinate;

        Vector2[] pointList = (Vector2[]) _uiLineRenderer.Points.Clone();
        pointList[1] = playerCoordinate;
        _uiLineRenderer.Points = pointList;
    }

    private void OnEnable()
    {
        TimeManager.OnStarted += ResetScalar;
    }
    
    private void OnDisable()
    {
        TimeManager.OnStarted -= ResetScalar;
    }

    private void ResetScalar()
    {
        _initializedScalar = false;
    }
}