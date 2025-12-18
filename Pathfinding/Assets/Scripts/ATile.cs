using TMPro;
using UnityEngine;

// Tile for A*-algorith
//[RequireComponent(typeof(TextMeshPro))]
public class ATile : MonoBehaviour
{
    // Smallest possible cost from path start to this tile
    [SerializeField]
    private int FCost = -1; // Total estimated F = G + H
    [SerializeField]
    private int GCost = -1; //real Cost from start
    [SerializeField]
    private int HCost = -1; // Heuristic Cost to goal
    

    public bool isWalkable = true;

    // Coordinates for this tile in the grid
    public Vector2Int Pos = new Vector2Int(-1, -1);
    // Where did we come from
    public Vector2Int CameFrom = new Vector2Int(-1, -1);

    //Just for accessing and modifying the text component
    public TextMeshPro myTMPFCost;
    public TextMeshPro myTMPGCost;
    public TextMeshPro myTMPHCost;
    private bool isDirty = true;

    // For accessing and modifying the material colour
    private MeshRenderer myMR;

    public void ComputeHCost(Vector2Int _target)
    {
        Vector2Int diff = _target - this.Pos;        
        this.HCost =(int) (10.0f* diff.magnitude);
        isDirty = true;
    }

    // Cost getter
    public int GetFCost() 
    {
        return this.FCost;
    }

    public int GetGCost() 
    {
        return this.GCost;
    }

    // Cost must be updated using this function
    public void SetGCost(int _cost) 
    {
        //handle infinite
        if(_cost == int.MaxValue)
        {
            this.GCost = int.MaxValue;
            this.FCost = int.MaxValue;
            isDirty = true;
            return;
        }
        this.GCost = _cost;
        this.FCost = this.GCost + this.HCost;
        isDirty = true;
    }

    public void SetColor(Color _color) 
    {
        if (myMR != null)
        {
            myMR.material.color = _color;
        }
        else 
        {
            Debug.Log("MeshRenderer not found");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Populate my TMP at start
        //myTMP = GetComponentInChildren<TextMeshPro>();

        //Populate myMat at start
        myMR = GetComponent<MeshRenderer>();
    }

    void UpdateFCostText()
    {
        if(myTMPFCost != null)
        {        
            if (FCost == int.MaxValue) 
            {
                myTMPFCost.text = "-1";
            }
            else
            {
                myTMPFCost.text = FCost.ToString();
            }  
            
            if (!isWalkable)
            {
                myTMPFCost.text = "X";
                myMR.material.color = Color.black;
            }
                   
        }
        else if (myTMPFCost == null)
        {
            Debug.Log("TectMeshPro component for FCost not found!");
        }
    }

void UpdateGCostText()
    {
        if(myTMPGCost != null)
        {        
            if (GCost == int.MaxValue) 
            {
                myTMPGCost.text = "-1";
            }
            else
            {
                myTMPGCost.text = GCost.ToString();
            }           
        }
        else if (myTMPGCost == null)
        {
            Debug.Log("TectMeshPro component for GCost not found!");
        }
    }

    void UpdateHCostText()
    {
        if(myTMPHCost != null)
        {        
            if (HCost == int.MaxValue) 
            {
                myTMPHCost.text = "-1";
            }
            else
            {
                myTMPHCost.text = HCost.ToString();
            }     
        }
        else if (myTMPHCost == null)
        {
            Debug.Log("TectMeshPro component for HCost not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDirty)
        {
            UpdateFCostText();
            UpdateGCostText();
            UpdateHCostText();
            isDirty = false;
        }
        
    }
}
