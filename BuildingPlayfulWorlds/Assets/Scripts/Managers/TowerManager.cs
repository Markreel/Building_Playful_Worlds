using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField]
    List<Tower> towers = new List<Tower>();
    [SerializeField]
    List<Connection> connections = new List<Connection>();

    void Start()
    {
        SetupLines();
    }

    /// <summary>
    /// Sets the positions for the connection lines
    /// </summary>
    void SetupLines()
    {
        foreach (var _connection in connections)
        {
            _connection.LineRenderer.SetPosition(0, towers[_connection.TowerAIndex].Orb.transform.position);
            _connection.LineRenderer.SetPosition(1, towers[_connection.TowerBIndex].Orb.transform.position);
            _connection.LineRenderer.enabled = false;
        }

        foreach (var _tower in towers)
        {
            _tower.Orb.SetActive(false); 
        }
    }

    public void CheckTriggers(GameObject _trigger)
    {
        foreach (var _tower in towers)
        {
            if (_tower.Trigger == _trigger)
            {
                _tower.isOn = true;
                _tower.Orb.SetActive(true);
                _tower.Trigger.SetActive(false);
            }
        }

        CheckIfWon();
        UpdateConnections();
    }
    
    void CheckIfWon()
    {
        bool _won = true;

        foreach (var _tower in towers)
        {
            if (!_tower.isOn)
                _won = false;
        }

        if (_won)
            GameManager.Instance.Victory();
    }

    void UpdateConnections()
    {
        foreach (var _connection in connections)
        {
            if (towers[_connection.TowerAIndex].isOn && towers[_connection.TowerBIndex].isOn)
                _connection.LineRenderer.enabled = true;
        }
    }
}

[System.Serializable]
public class Tower
{
    public GameObject Orb;
    public GameObject Trigger;
    public bool isOn;
}

[System.Serializable]
public class Connection
{
    public int TowerAIndex;
    public int TowerBIndex;
    public LineRenderer LineRenderer;
}
