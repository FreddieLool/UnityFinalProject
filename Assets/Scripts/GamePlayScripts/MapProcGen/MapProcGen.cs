using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Diagnostics;


public class MapProcGen : MonoBehaviour
{
    private bool[,] _mapSolidsArr;
    private int 
        _mapWidth = 40,
        _mapHeight = 35,
        _mapObjAmount = 255,
        _enemyUnitAmount = 1,
        _tries = 0,
        _limit = 333333,
        _rndX, _rndY,
        _enemySpawnMill = 500,
        _enemyMaxAmount = 20;

    public static int CurrentEnemyAmount { get; private set; }

    private Stopwatch _enemySpawnTimer = new Stopwatch();

    private void Start()
    {
        _mapSolidsArr = new bool[_mapWidth, _mapHeight];

        // GENERATING MAP OBJECTS :
        GenerateSomething(OBJ_TAG.MAP_OBJ, _mapObjAmount);
    }

    private void FixedUpdate()
    {
        if(CurrentEnemyAmount <= _enemyMaxAmount && !_enemySpawnTimer.IsRunning 
            || _enemySpawnTimer.ElapsedMilliseconds >= _enemySpawnMill)
        {
            GenerateSomething(OBJ_TAG.UNIT, _enemyUnitAmount);
            CurrentEnemyAmount += _enemyUnitAmount;
            _enemySpawnTimer.Restart();
        }
    }

    private void GenerateSomething(OBJ_TAG objTag , int Amount)
    {
        for (int i = 0; i < Amount; i++)
        {
            ApplyRandomPos();
            GameObject randomPrfb = GetRandomPrefab(objTag);

            if (IsGoodGenerationHelper(randomPrfb))
            {
                AddPrefabToMap(randomPrfb);
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

    private bool OutOfBounds(int x , int y) // returns true of the pos given is out of the terrain.
    { return x < 0 || y < 0 || x > (_mapWidth - 2) || y > (_mapHeight - 2) ; }

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

        Vector2 pos = (v2List.ToArray()[0] + new Vector2(_rndX, _rndY)) * (ProcGen.V_SIZE * new Vector2(1 , -1));
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
    public static readonly float SIZE = 6.5f;
    public static readonly Vector2 V_SIZE = Vector2.one * SIZE;

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

                     {
                        "Enemy-Default2",
                        Resources.Load("Prefabs/Units/Enemy/Enemy-Default", typeof(GameObject)) as GameObject
                     },

                     {
                        "Enemy-Default3",
                        Resources.Load("Prefabs/Units/Enemy/Enemy-Default", typeof(GameObject)) as GameObject
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
    };
}


public enum OBJ_TAG
{
    MAP_OBJ,
    UNIT,
}