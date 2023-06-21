using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Diagnostics;
using System.Reflection;





public class MapProcGen : MonoBehaviour
{
    private bool[,] _mapSolidsArr;
    private int
        _mapWidth = 57, // height of the map.
        _mapHeight = 50, // width of the map.
        _mapObjAmount = 505, // how much map object to spawn on the map.
        _tries = 0, // current tries.
        _limit = 333333, // limit on how many tries the procGen methon can try to generate an object.
        _rndX, _rndY, // random cords for the proc gen.
        _enemyAmount = 1, // how much enemies spawn every resMill.
        _enemyResMill = 250, // amount of time for each enemy to respawn.
        _enemyMinSpawnDistance = 25, // min dist that enemy can spawn from the player.
        _maxEnemiesOnTheMap = 50; // max amount of enemy units that can be on the map. 

    private static float _size = 4.5F; // density of the peoc gen ( the less this num is , the more dense object will be to one another).
    private Vector2 _v_size = Vector2.one * _size; // size in Vector2.

    private Stopwatch _enemyResTim = new Stopwatch();
    private GameObject _player;

    // GENERAL USE METHODS!
    private bool IsPosOnEdgeOfMap(int x, int y) // returns true if the selected tile pos is on the edge of the map.
    { return x == 0 || y == 0 || x == _mapWidth - 1 || y == _mapHeight - 1; }
    private bool OutOfBounds(int x, int y) // returns true of the pos given is out of the terrain.
    { return x < 0 || y < 0 || x > (_mapWidth - 2) || y > (_mapHeight - 2); }
    //--

    private void Start()
    {
        _player = GameObject.Find("Player");

        _mapSolidsArr = new bool[_mapWidth, _mapHeight];
        // GENERATING RING OB BORDERS:
        GenerateBorders();

        // GENERATING MAP OBJECTS :
        GenerateSomething(OBJ_TAG.MAP_OBJ, _mapObjAmount);

        _enemyResTim.Start();
    }

    private void FixedUpdate()
    {
        if(_enemyResTim.ElapsedMilliseconds >= _enemyResMill)
        {
            GenerateSomething(OBJ_TAG.UNIT, _enemyAmount ,
                _enemyMinSpawnDistance);

            _enemyResTim.Restart();
        }
    }

    private void GenerateBorders()
    {
        GameObject BorderPrefab = ProcGen.PrefabFetcherDic[OBJ_TAG.BORDER]["Border-Default"];
        BorderPrefab.transform.localScale = new Vector2(0.52f * _size, 0.52f * _size);
        for (int iX = 0; iX < _mapWidth; iX++)
        {
            for(int iY = 0; iY < _mapHeight; iY++)
            {
                if(IsPosOnEdgeOfMap(iX , iY)) 
                {
                    _rndX = iX;
                    _rndY = iY;
                    AddPrefabToMap(BorderPrefab);
                }
            }
        }
    }

    private void GenerateSomething(OBJ_TAG objTag , int Amount , int minDistance = 0)
    {
        bool IsSolid = true;
        if(objTag == OBJ_TAG.UNIT) { IsSolid = !IsSolid; }

        for (int i = 0; i < Amount; i++)
        {
            ApplyRandomPos();

            float dist = Vector2.Distance(new Vector2(_rndX, _rndY), _player.transform.position);
            if (minDistance != 0)
            {
                while (dist <= minDistance)
                {
                    ApplyRandomPos();
                    dist = Vector2.Distance(new Vector2(_rndX, _rndY), _player.transform.position);
                }
            }   
            
            GameObject randomPrfb = GetRandomPrefab(objTag);

            if (IsGoodGenerationHelper(randomPrfb))
            {
                AddPrefabToMap(randomPrfb , IsSolid);
                _tries = 0;
            }
            else
            {
                _tries++;
                if (_tries >= _limit)
                {
                    _tries = 0;
                    return;
                }
            }
        }
    }


    private bool IsGoodGenerationHelper(GameObject randomPrfb)
    {
        List<Vector2> v2List = new List<Vector2>(ProcGen.PrefabSizeDic[randomPrfb.name]);
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

    private bool IsGoodTilePlacement(int x, int y , int min , int max) 
    {
        return !_mapSolidsArr[x,y] && ((TileCrossChecker(x,y) == min && TileDiagonalChecker(x,y) <= max) 
            || (TileCrossChecker(x,y) <= max && TileDiagonalChecker(x,y) == min)); 
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

    private void AddPrefabToMap(GameObject randomPrfb , bool IsSolid = true)
    {
        List<Vector2> v2List = new List<Vector2>(ProcGen.PrefabSizeDic[randomPrfb.name]);
        int prefabSize = v2List.Count;

        foreach(Vector2 v2 in v2List)
        {
            _mapSolidsArr[(int)v2.x + _rndX, (int)v2.y + _rndY] = IsSolid;
        }

        Vector2 pos = (v2List.ToArray()[0] + new Vector2(_rndX, _rndY)) * (_v_size * new Vector2(1 , -1));
        SpriteRenderer sprite = randomPrfb.GetComponent<SpriteRenderer>();
        sprite.sortingOrder = ((int)pos.y * -1) + 10;
        Instantiate(randomPrfb, pos, Quaternion.identity);
    }


    // gets an gameObj instan of a random prefab from the dic pool:
    private GameObject GetRandomPrefab(OBJ_TAG objTag)
    {
        int rnd = Random.Range(0, ProcGen.PrefabFetcherDic[objTag].Keys.Count);
        return ProcGen.PrefabFetcherDic[objTag]
            [ProcGen.PrefabFetcherDic[objTag].Keys.ToArray()[rnd]];
    }

    private void ApplyRandomPos()
    {
        _rndX = Random.Range(0, _mapWidth-1);
        _rndY = Random.Range(0, _mapHeight-1);
    }
}




public interface ProcGen
{
    public static readonly Dictionary<OBJ_TAG, Dictionary<string, GameObject>> PrefabFetcherDic = new Dictionary<OBJ_TAG, Dictionary<string, GameObject>>()
    {
        {
              OBJ_TAG.MAP_OBJ, new Dictionary<string, GameObject>
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
              }
        },

        {
              OBJ_TAG.UNIT, new Dictionary<string, GameObject>
              {
                     {
                        "Enemy-Default",
                        Resources.Load("Prefabs/Units/Enemy/Enemy-Default", typeof(GameObject)) as GameObject
                     },
              }

        },

        {
            OBJ_TAG.BORDER, new Dictionary<string, GameObject>
            {
                     {
                        "Border-Default",
                        Resources.Load("Prefabs/Environment/Border-Default", typeof(GameObject)) as GameObject
                     },
            }
        },

    };
    public static readonly Dictionary<string, List<Vector2>> PrefabSizeDic = new Dictionary<string, List<Vector2>>
    {
         {
            "Enemy-Default",
            new List<Vector2>
            {
                new Vector2(0,0),
            }
        },

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

        {
            "Border-Default",
            new List<Vector2>
            {
                new Vector2(0,0),
            }
        },
    };
}


public enum OBJ_TAG
{
    MAP_OBJ,
    UNIT,
    BORDER,
}