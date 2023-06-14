using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MapProcGen : MonoBehaviour
{
    private bool[,] _mapSolidsArr;
    private int _mapWidth = 20, _mapHeight = 20 , _objAmount = 75 , _tries = 0 , _limit = 500 ,
        _rndX , _rndY;


    


    private void Start()
    {
        _mapSolidsArr = new bool[_mapWidth, _mapHeight];

        MapGeneration();
    }

    private void MapGeneration()
    {
        for(int i = 0; i < _objAmount ; i++)
        {
            ApplyRandomPos();
            GameObject randomPrfb = GetRandomPrefab();

            if(IsGoodGenerationHelper(randomPrfb))
            {
                AddPrefabToMap(randomPrfb);
            }
            else
            {
                i--;
                _tries++;
                if(_tries >= _limit)
                {
                    return;
                }
            }
        }
    }


    private bool IsGoodGenerationHelper(GameObject randomPrfb)
    {
        List<Vector2> v2List = ProcGen.PrefabSizeDic[randomPrfb.name];
        int prefabSize = v2List.Count;
        Vector2 v2 = ProcGen.PrefabSizeDic[randomPrfb.name].ToArray()[0];
        int x = (int)v2.x + _rndX;
        int y = (int)v2.y + _rndY;

        if(OutOfBounds(x,y))
        {
            return false;
        }

        if (prefabSize <= 1)
        {    
            return IsGoodTilePlacement(x, y, 0, 1);
        }

        return IsGoodGeneration(v2List);
    }

    private bool IsGoodGeneration(List<Vector2> v2List)
    {
        int x = (int)v2List.ToArray()[0].x + _rndX;
        int y = (int)v2List.ToArray()[0].y + _rndY;
        if (OutOfBounds(x, y))
        {
            return false;
        }

        if (IsGoodTilePlacement(x , y , 0 , 0))
        {
            if(v2List.Count <= 1)
            {
                return true;
            }
            v2List.RemoveAt(0);
        }
        else
        {
            return false;
        }
        
        return IsGoodGeneration(v2List); 
    }

    private bool OutOfBounds(int x , int y) // returns true of the pos given is out of the terrain.
    { return x < 0 || y < 0 || x > (_mapWidth - 2) || y > (_mapHeight - 2) ; }

    private bool IsGoodTilePlacement(int x, int y , int min , int max) 
    {
        return ((TileCrossChecker(x,y) == min && TileDiagonalChecker(x,y) <= max)) 
            || (TileCrossChecker(x,y) <= max && TileDiagonalChecker(x,y) == min); 
    }

    private int TileCrossChecker(int x , int y)
    {
        int answer = 0;
        bool[] TileCrossArr =
        {
            (Bans(x + 1, y)) ,(Bans(x -1 , y)) ,
            (Bans(x , y -1)) ,(Bans(x , y +1)) ,
        };
        foreach (bool b in TileCrossArr)
        { 
            if (!b)
            {
                answer++;
            }
        }

        return answer;
    }
    private int TileDiagonalChecker(int x, int y)
    {
        int answer = 0;
        bool[] TileDiagonalArr =
        {
            (Bans(x + 1, y +1)) ,(Bans(x -1 , y +1)) ,
            (Bans(x +1 , y -1)) ,(Bans(x -1 , y -1)) ,
        };

        foreach (bool b in TileDiagonalArr)
        {
            if (!b)
            {
                answer++;
            }
        }
        return answer;
    }

    private bool Bans(int x , int y)
    {
        if (OutOfBounds(x, y)) { return false; }
        else {return !_mapSolidsArr[x , y ]; }
    }

    private void AddPrefabToMap(GameObject randomPrfb)
    {
        List<Vector2> v2List = ProcGen.PrefabSizeDic[randomPrfb.name];
        int prefabSize = v2List.Count;

        foreach(Vector2 v2 in v2List)
        {
            _mapSolidsArr[(int)v2.x, (int)v2.y] = true;
        }

        Vector2 pos = (v2List.ToArray()[0] + new Vector2(_rndX, _rndY)) * (ProcGen.V_SIZE * new Vector2(1 , -1));
        SpriteRenderer sprite = randomPrfb.GetComponent<SpriteRenderer>();
        sprite.sortingOrder = (int)pos.y * -1;
        Instantiate(randomPrfb, pos, Quaternion.identity);
    }


    // gets an gameObj instan of a random prefab from the dic pool:
    private GameObject GetRandomPrefab()
    {
        int rnd = (int)Random.Range(0, ProcGen.PrefabSizeDic.Keys.Count);
        return ProcGen.PrefabFetcherDic
        [ProcGen.PrefabFetcherDic.Keys.ToArray()[rnd]];


        //FOR NOW TESTING !
        //return ProcGen.PrefabFetcherDic["Tree-dried"];
    }

    private void ApplyRandomPos()
    {
        _rndX = (int)Random.Range(0, _mapWidth-1);
        _rndY = (int)Random.Range(0, _mapHeight-1);
    }
}

public interface ProcGen
{
    public static readonly float SIZE = 6.5f;
    public static readonly Vector2 V_SIZE = Vector2.one * SIZE;
    
    public static readonly Dictionary<string, GameObject> PrefabFetcherDic = new Dictionary<string, GameObject>
    {
        {
            "Tree-orange", 
            Resources.Load("Prefabs/Environment/Tree-orange", typeof(GameObject)) as GameObject 
        },

        {
            "Tree-dried" ,
            Resources.Load("Prefabs/Environment/Tree-dried", typeof(GameObject)) as GameObject 
        },

        {
            "Rock-monument" , 
            Resources.Load("Prefabs/Environment/Rock-monument", typeof(GameObject)) as GameObject 
        },

    };

    public static readonly Dictionary<string, List<Vector2>> PrefabSizeDic = new Dictionary<string, List<Vector2>>
    {
        {
            "Tree-orange",
            new List<Vector2>
            {
                new Vector2(0 , 0),
                new Vector2(1 , 0),
            }
        },

        {
            "Tree-dried" ,
            new List<Vector2>
            {
                new Vector2(0 , 0),
            }
        },

        {
            "Rock-monument" ,
            new List<Vector2>
            {
                new Vector2(0 , 0),
                new Vector2(0 , 1),
                new Vector2(1 , 0),
                new Vector2(1 , 1),
            }
        },

    };


}


