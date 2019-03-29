using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    [SerializeField] private LineRenderer laserLineRenderer;
    [SerializeField] private float maxLaserLenght = 50;
    [SerializeField] GameObject laserTargetEffect;
    [SerializeField] GameObject laserTargetEffectCollision;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            laserLineRenderer.enabled = true;
            DrawLaserFromTargetPosition(new Vector3(transform.localPosition.x + laserLineRenderer.transform.right.x/2, transform.localPosition.y + 0.2f, transform.localPosition.z + laserLineRenderer.transform.right.z / 2), laserLineRenderer.transform.forward, maxLaserLenght);
        }
        else
        {
            laserTargetEffect.SetActive(false);
            laserTargetEffectCollision.SetActive(false);
            laserLineRenderer.enabled = false;
        }
    }

    void DrawLaserFromTargetPosition(Vector3 _targetPosition, Vector3 _direction, float _length)
    {
        Ray _ray = new Ray(_targetPosition, _direction);
        RaycastHit _raycastHit;
        Vector3 _endPosition = _targetPosition + (_length * _direction);

        if (Physics.Raycast(_ray, out _raycastHit, _length))
        {
            if (_raycastHit.collider.tag != "Player")
                _endPosition = _raycastHit.point;

            laserTargetEffect.transform.position = _targetPosition;
            laserTargetEffectCollision.transform.position = _targetPosition;

            laserTargetEffect.SetActive(false);
            laserTargetEffectCollision.SetActive(true);
            //laserTargetEffect.transform.rotation.SetFromToRotation(laserLineRenderer.transform.forward + _targetPosition, _endPosition);
        }

        else
        {
            laserTargetEffect.SetActive(true);
            laserTargetEffectCollision.SetActive(false);
        }

        laserLineRenderer.SetPosition(0, _targetPosition);
        laserLineRenderer.SetPosition(1, _endPosition);
    }


    //LERP PARTICLE DURATION VAN 1 NAAR 0.1F!!!
}
