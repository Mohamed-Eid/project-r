using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnitSelectionManager.Instance.AllUnits.Add(gameObject);
    }


    private void OnDestroy()
    {
        UnitSelectionManager.Instance.AllUnits.Remove(gameObject);
        UnitSelectionManager.Instance.SelectedUnits.Remove(gameObject);
    }

}
