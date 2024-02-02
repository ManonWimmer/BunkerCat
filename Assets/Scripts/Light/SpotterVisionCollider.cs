using System.Collections.Generic;
using UnityEngine;

public class SpotterVisionCollider : MonoBehaviour
{
    [SerializeField]
    private Spotter _spotter;

    private Collider2D _collider;
    private bool _playerInCollider;
    private List<Collider2D> _overlappedColliders;
    private ContactFilter2D _contactFilter;
    //=================================================================

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _overlappedColliders = new List<Collider2D>();

        _contactFilter = new();
        _contactFilter.SetLayerMask(LayerMask.GetMask("Player"));
        _contactFilter.ClearDepth();
        _contactFilter.ClearNormalAngle();
    }

    private void FixedUpdate()
    {
        int count = Physics2D.OverlapCollider(_collider, _contactFilter, _overlappedColliders);
        if (count > 0)
        {
            _spotter.PlayerInSight(_overlappedColliders[0].transform.position);
            _playerInCollider = true;
        }
        else if (_playerInCollider)
        {
            _spotter.PlayerOutOfSight();
            _playerInCollider = false;
        }
    }
}
