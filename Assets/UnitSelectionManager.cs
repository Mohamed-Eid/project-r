using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSelectionManager : MonoBehaviour
{

    Camera cam;
    NavMeshAgent agent;

    LayerMask clickableMask;
    LayerMask groundMask;
    public GameObject groundmarker;

    public static UnitSelectionManager Instance { get; private set; }

    public List<GameObject> AllUnits { get; set; } = new List<GameObject>();
    public List<GameObject> SelectedUnits { get; set; } = new List<GameObject>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        groundMask = LayerMask.GetMask("Ground");
        clickableMask = LayerMask.GetMask("Clickable");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //if we hitting a clickable object
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickableMask))
            {

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    MultipleSelect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }

            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    //Deselct all units
                    DeselectAllUnits();
                }
            }
        }

        //update the ground marker
        if (Input.GetMouseButtonDown(1) && SelectedUnits.Count > 0)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
            {
                groundmarker.transform.position = hit.point;
                groundmarker.SetActive(false);
                groundmarker.SetActive(true);
            }
        }
    }

    private void DeselectAllUnits()
    {
        foreach (GameObject unit in SelectedUnits)
        {
            EnableUnitMovement(unit, false);
            TriggerSelectionIndicator(unit, false);
        }
        SelectedUnits.Clear();
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAllUnits();
        SelectedUnits.Add(unit);
        TriggerSelectionIndicator(unit, true);
        EnableUnitMovement(unit, true);
    }

    private void MultipleSelect(GameObject unit)
    {
        if (!SelectedUnits.Contains(unit))
        {
            SelectedUnits.Add(unit);
            TriggerSelectionIndicator(unit, true);
            EnableUnitMovement(unit, true);
        }
        else
        {
            EnableUnitMovement(unit, false);
            TriggerSelectionIndicator(unit, false);
            SelectedUnits.Remove(unit);
        }

    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
        unit.GetComponent<UnitMovement>().enabled = shouldMove;
    }


    private void TriggerSelectionIndicator(GameObject unit, bool isVisibile)
    {
        unit.transform.Find("Indicator").gameObject.SetActive(isVisibile);
    }
}
