using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance;

    public bool inBuildMode = false;

    public GameObject[] buildingPieces;
    public int selectedPieceIndex = 0;

    private GameObject ghostObject;
    public Material validMaterial;
    public Material invalidMaterial;

    private bool canPlace = false;
    public float buildDistance = 10f;

    public LayerMask buildLayerMask;
    public Camera buildCamera;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Update()
    {
        if (!inBuildMode) return;
          Debug.Log("In build mode! Ghost: " + ghostObject);
        UpdateGhostPosition();

        if (Input.GetKeyDown(KeyCode.Q))
            CyclePiece(-1);
        if (Input.GetKeyDown(KeyCode.E) && !InventorySystem.Instance.isOpen)
            CyclePiece(1);

        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            Debug.Log("Placing piece!");
            PlacePiece();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left click but canPlace is: " + canPlace);
        }
    }

    public void EnterBuildMode()
    {
         Debug.Log("Entering build mode! Pieces count: " + buildingPieces.Length);
        inBuildMode = true;
        SpawnGhost();
    }

    public void ExitBuildMode()
    {
        inBuildMode = false;
        if (ghostObject != null)
            Destroy(ghostObject);
    }

    void SpawnGhost()
    {
       
        if (ghostObject != null)
            Destroy(ghostObject);

        ghostObject = Instantiate(buildingPieces[selectedPieceIndex]);
         
        
        Collider col = ghostObject.GetComponent<Collider>();
        if (col != null) col.isTrigger = true;

        SetGhostMaterial(invalidMaterial);
    }

    void UpdateGhostPosition()
    {
        Ray ray = buildCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
    bool didHit = Physics.Raycast(ray, out hit, buildDistance, buildLayerMask);
    Debug.Log("Placing piece!"); Debug.Log("Placing piece!"); Debug.Log("Placing piece!"); Debug.Log("Placing piece!"); Debug.Log("Placing piece!"); Debug.Log("Placing piece!");
    if (didHit) Debug.Log("Hit: " + hit.collider.name);
        if (Physics.Raycast(ray, out hit, buildDistance, buildLayerMask))
        {
            // Clamp the position so it never goes further than buildDistance
            Vector3 placementPos = ray.origin + ray.direction * Mathf.Min(hit.distance, buildDistance);
            
            ghostObject.SetActive(true);
            ghostObject.transform.position = placementPos + new Vector3(0, ghostObject.transform.localScale.y / 2, 0);

            Collider[] overlaps = Physics.OverlapBox(
                ghostObject.transform.position,
                ghostObject.transform.localScale / 2
            );

            bool overlapping = false;
            foreach (Collider col in overlaps)
            {
                if (col.gameObject != ghostObject)
                {
                    overlapping = true;
                    break;
                }
            }

            if (!overlapping)
            {
                canPlace = true;
                SetGhostMaterial(validMaterial);
            }
            else
            {
                canPlace = false;
                SetGhostMaterial(invalidMaterial);
            }
        }
        else
        {
            // Even if no hit, show ghost at max build distance
            ghostObject.SetActive(true);
            ghostObject.transform.position = ray.origin + ray.direction * buildDistance;
            canPlace = false;
            SetGhostMaterial(invalidMaterial);
        }
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        float gridSize = 3f;
        pos.x = Mathf.Round(pos.x / gridSize) * gridSize;
        pos.z = Mathf.Round(pos.z / gridSize) * gridSize;
        return pos;
    }

    void PlacePiece()
    {
        BuildingPiece piece = buildingPieces[selectedPieceIndex].GetComponent<BuildingPiece>();
        Debug.Log("Trying to place: " + piece.pieceName);
        Debug.Log("Can place: " + canPlace);
        Debug.Log("Has materials: " + HasMaterials(piece));

        if (!HasMaterials(piece))
        {
            Debug.Log("Not enough materials!");
            return;
        }

        ConsumeMaterials(piece);
        Instantiate(buildingPieces[selectedPieceIndex], ghostObject.transform.position, ghostObject.transform.rotation);
        Debug.Log("Placed: " + piece.pieceName);
    }

    bool HasMaterials(BuildingPiece piece)
    {
        int logs = CountItem("Log");
        int planks = CountItem("Plank");
        int stones = CountItem("Stone");

        Debug.Log("Logs: " + logs + " Planks: " + planks + " Stones: " + stones);
        Debug.Log("Need - Logs: " + piece.logCost + " Planks: " + piece.plankCost + " Stones: " + piece.stoneCost);

        return logs >= piece.logCost && planks >= piece.plankCost && stones >= piece.stoneCost;
    }

    void ConsumeMaterials(BuildingPiece piece)
    {
        if (piece.logCost > 0) InventorySystem.Instance.RemoveItem("Log", piece.logCost);
        if (piece.plankCost > 0) InventorySystem.Instance.RemoveItem("Plank", piece.plankCost);
        if (piece.stoneCost > 0) InventorySystem.Instance.RemoveItem("Stone", piece.stoneCost);
    }

    int CountItem(string itemName)
    {
        int count = 0;
        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == itemName) count++;
        }
        return count;
    }

    void CyclePiece(int direction)
    {
        selectedPieceIndex += direction;
        if (selectedPieceIndex < 0) selectedPieceIndex = buildingPieces.Length - 1;
        if (selectedPieceIndex >= buildingPieces.Length) selectedPieceIndex = 0;
        SpawnGhost();
    }

    void SetGhostMaterial(Material mat)
    {
        Renderer[] renderers = ghostObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
            r.material = mat;
    }
}