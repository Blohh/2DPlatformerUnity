using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;
    public Transform levelStartPoint;
    public List<LevelPieceBasic> levelPrefabs = new List<LevelPieceBasic>();
    public List<LevelPieceBasic> pieces = new List<LevelPieceBasic>();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        AddPiece();
        AddPiece();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddPiece()
    {
        int randomIndex = Random.Range(0, levelPrefabs.Count - 1);
        LevelPieceBasic piece = (LevelPieceBasic)Instantiate(levelPrefabs[randomIndex]);
        piece.transform.SetParent(this.transform, false);
        if(pieces.Count < 1)
        {
            piece.transform.position = new Vector2(
                levelStartPoint.position.x - piece.startPoint.localPosition.x,
                levelStartPoint.position.y - piece.startPoint.localPosition.y);
        }
        else
        {
            piece.transform.position = new Vector2(
                pieces[pieces.Count - 1].exitPoint.position.x - piece.startPoint.localPosition.x + 2,
                pieces[pieces.Count - 1].exitPoint.position.y - piece.startPoint.localPosition.y);
        }
        pieces.Add(piece);
    }
    public void RemoveOldestPiece()
    {
        if (pieces.Count > 1)
        {
            LevelPieceBasic piece = pieces[0];
            pieces.RemoveAt(0);
            Destroy(piece.gameObject);
        }
    }
}
