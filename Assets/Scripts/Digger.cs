using UnityEngine;
using System.Collections;

public class Digger : MonoBehaviour
{
    private Manager _managerInstance;
    private int _rotation;
    private Vector2 _pos;
    private GenerationData _generationData;

    public void Initialize(Manager manager, Vector2 pos, GenerationData generationData)
    {
        //Get Serialized Generation Data
        _generationData = generationData;
        _managerInstance = manager;
        _pos = pos;
        _rotation = Random.Range(0, 3) * 90;

        //Begin Private Update Event
        StartCoroutine(DiggerUpdate());
    }

    public IEnumerator DiggerUpdate()
    {
        //Continue until max-tiles are reached (or you're destroyed);
        while (_managerInstance.TileCount < _managerInstance.MaxTiles)
        {
            //Create Floorsor diggers;
            CreateFloor(Random.Range(0, 3));
            DiggerSpawner(Random.Range(0, 100));
            //Update Transform;
            SetNewPosition();
            SetNewRotation(Random.Range(0, 100));
            yield return new WaitForEndOfFrame();
        }

        yield break;
    }

    private void SetNewRotation(int chanse)
    {
        //Set new rotation;
        if (chanse < _generationData.ReverseChanse) _rotation = (_rotation + 180) % 360;
        else if (chanse < _generationData.RotRightChanse) _rotation = (_rotation + 90) % 360;
        else if (chanse < _generationData.RotLeftChanse) _rotation = (_rotation + 270) % 360;
        //Else do nothing

        //Check if still inside bounds
        Vector2 newPos = GetNewPosition();
        if(newPos.x < 0 || newPos.y < 0 || newPos.x >= _managerInstance.Width || newPos.y >= _managerInstance.Height)
        {
            //If not, goto beginning of function;
            SetNewRotation(Random.Range(0, 100));
        }
    }

    private Vector2 GetNewPosition()
    {
        //Returns the next position for the given rotation;
        Vector2 localPos;
        localPos = new Vector2(_pos.x + Mathf.Round(Mathf.Cos(_rotation * Mathf.Deg2Rad)), _pos.y + Mathf.Round(Mathf.Sin(_rotation * Mathf.Deg2Rad)));

        return localPos;
    }

    private void SetNewPosition()
    {
        //Move to next location;
        _pos.x += Mathf.Round(Mathf.Cos(_rotation * Mathf.Deg2Rad));
        _pos.y += Mathf.Round(Mathf.Sin(_rotation * Mathf.Deg2Rad));

        transform.position = _pos;
    }

    private void CreateFloor(int size)
    {
        switch (size)
        {
            case (0):
                //Place single tile at position;
                _managerInstance.AddTile(_pos);
                break;

            case (1):
                //Place 2x2 field of tiles at position;
                for (int iX = 0; iX < 2; iX++)
                {
                    for (int iY = 0; iY < 2; iY++)
                    {
                        _managerInstance.AddTile(new Vector2(_pos.x + iX, _pos.y + iY));
                    }
                }
                break;

            case (2):
                //place 3x3 field of tiles at position;
                for (int iX = -1; iX < 2; iX++)
                {
                    for (int iY = -1; iY < 2; iY++)
                    {
                        _managerInstance.AddTile(new Vector2(_pos.x + iX, _pos.y + iY));
                    }
                }
                break;
        }
    }

    private void DiggerSpawner(float chanse)
    {
        //Make new Digger;
        if (chanse < _managerInstance.NewDiggerChanse)
        {
            _managerInstance.CreateNewDigger(_pos);
        }

        //Destroy self;
        if(chanse > _managerInstance.DiggerDestructionChanse)
        {
            DestroyDigger();
        }
    }

    private void DestroyDigger()
    {
        //destroy self;
        _managerInstance.DestroyDigger(this);
    }

    private void OnApplicationQuit()
    {
        //stop private update for digger;
        StopCoroutine(DiggerUpdate());
    }
}
