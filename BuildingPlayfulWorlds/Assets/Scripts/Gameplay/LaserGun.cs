using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    [SerializeField] private LineRenderer laserLineRenderer;
    [SerializeField] private float maxLaserLenght = 50;
    [SerializeField] GameObject laserTargetEffect;
    [SerializeField] GameObject laserTargetEffectCollision;

    [Header("Shoot Settings: ")] [Range(0,2)]
    [SerializeField] float shootDelay;
    [SerializeField] float damageAmount;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject barrel;

    float shootTimer;
    bool canShoot = true;

    private void Start()
    {
        laserLineRenderer.enabled = true;
        shootTimer = shootDelay;
    }

    void Update()
    {
        //DrawLaserFromTargetPosition(new Vector3(transform.localPosition.x + laserLineRenderer.transform.right.x/2, transform.localPosition.y + 0.2f, transform.localPosition.z + laserLineRenderer.transform.right.z / 2), laserLineRenderer.transform.forward, maxLaserLenght);
        //DrawLaserFromTargetPosition(barrel.transform.position + new Vector3(0,0.05f,0), barrel.transform.forward, maxLaserLenght);

        if (Input.GetMouseButton(0) && canShoot)
        {
            Shoot();
        }

        if (!canShoot)
            CooldownTimer();
    }

    void CooldownTimer()
    {
        if (shootTimer > 0)
            shootTimer -= Time.deltaTime;
        else
            canShoot = true;
    }

    void Shoot()
    {
        shootTimer = shootDelay;
        canShoot = false;

        GameObject _obj = Instantiate(projectile, barrel.transform.position, Quaternion.Euler(projectile.transform.eulerAngles + barrel.transform.eulerAngles));
        _obj.GetComponent<Rigidbody>().AddForce(barrel.transform.forward * 250);
        _obj.GetComponent<Projectile>().DamageAmount = damageAmount;
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

            //laserTargetEffect.transform.position = _targetPosition;
            //laserTargetEffectCollision.transform.position = _targetPosition;

            //laserTargetEffect.SetActive(false);
            //laserTargetEffectCollision.SetActive(true);
            //laserTargetEffect.transform.rotation.SetFromToRotation(laserLineRenderer.transform.forward + _targetPosition, _endPosition);
        }

        else
        {
            //laserTargetEffect.SetActive(true);
            //laserTargetEffectCollision.SetActive(false);
        }

        laserLineRenderer.SetPosition(0, _targetPosition);
        laserLineRenderer.SetPosition(1, _endPosition);
    }
}
