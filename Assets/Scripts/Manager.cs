using UnityEngine;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private int _width, _height, _maxTiles;
    [SerializeField]
    private GenerationData _rotationChanses;
    [SerializeField]
    private GameObject _digger;
    private Floor[][] _tiles;
    private int _tileCount;
    private List<Digger> _diggerList;

    private int _diggerDestructionChanse = int.MaxValue;
    private int _newDiggerChanse;

    public int Width { get { return _width; } }
    public int Height { get { return _height; } }

    public int NewDiggerChanse { get { return _newDiggerChanse; } }
    public int DiggerDestructionChanse { get { return _diggerDestructionChanse; } }

    public int TileCount { get { return _tileCount; } }
    public int MaxTiles { get { return _maxTiles; } }

    private void Start()
    {
        //Set a unique random seed;
        Random.InitState((int)System.DateTime.Now.Ticks);

        //Create Digger & Tile maps;
        _diggerList = new List<Digger>();
        _tiles = new Floor[_height][];
        for (int i = 0; i < _tiles.Length; i++)
        {
            _tiles[i] = new Floor[_width];
        }
        
        //Create First Digger at center of array;
        CreateNewDigger(new Vector2(Mathf.Round(_width * 0.5f), Mathf.Round(_height * 0.5f)));
    }

    public void AddTile(Vector2 diggerPos)
    {
        if (diggerPos.x >= 0 && diggerPos.y >= 0 && diggerPos.x < _width && diggerPos.y < _height)
        {
            if (_tiles[Mathf.RoundToInt(diggerPos.x)][Mathf.RoundToInt(diggerPos.y)] != null)
            {
                //If there is alreay a tile on this position, exit;
                return;
            }

            //Create new tile on given position;
            _tiles[Mathf.RoundToInt(diggerPos.x)][Mathf.RoundToInt(diggerPos.y)] = new Floor(diggerPos, transform);
            _tileCount++;

            //If number of tiles is bigger than max-tiles, destroy all diggers;
            if (TileCount >= _maxTiles)
            {
                for (int i = 0; i < _diggerList.Count; i++)
                {
                    DestroyDigger(_diggerList[i]);
                }
            }
        }
    }

    public void CreateNewDigger(Vector2 pos)
    {
        //Create and initialize digger at current position;
        Digger digger = Instantiate(_digger).GetComponent<Digger>();
        digger.transform.position = pos;
        digger.Initialize(this, pos, _rotationChanses);
        _diggerList.Add(digger);

        //Recalculate create & destroy chance;
        RecalculateChanses();
    }

    public void DestroyDigger(Digger digger)
    {
        //Destroy digger if it still exists;
        if (digger != null)
        {
            _diggerList.Remove(digger);
            Destroy(digger.gameObject);

            //Recalculate create & destroy chance;
            RecalculateChanses();
        }
    }

    private void RecalculateChanses()
    {
        //Recalculate create & destroy chance;
        _diggerDestructionChanse = 102 - _diggerList.Count * 2;
        _newDiggerChanse = 10 - _diggerList.Count * 2;
    }
}
