using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Diagnostics;


public class MapProcGen : MonoBehaviour
{
    [SerializeField] GameObject Player;

    private bool[,] _mapSolidsArr;
    private int
        _mapWidth = 75, // height of the map.
        _mapHeight = 65, // width of the map.
        _mapSmallObjAmount = 80,
        _mapMedObjAmount = 155,
        _mapLargeObjAmount = 55,
        _tries = 0, // current tries.
        _limit = 333333, // limit on how many tries the procGen methon can try to generate an object.
        _rndX, _rndY, // random cords for the proc gen.
        _enemyAmount = 1, // how much enemies spawn every resMill.
        _enemyResMill = 1950, // amount of time for each enemy to respawn.
        _minEnemyResMill = 150,
        _enemyResMillReduc = 2,
        _enemyMinSpawnDistance = 30, // min dist that enemy can spawn from the player.
        _enemyMaxSpawnDistance = 60,
        _treasureChestSpawnMill = 15000;
        //_maxEnemiesOnTheMap = 50; // max amount of enemy units that can be on the map.

    private static float _size = 3.375F; // density of the peoc gen ( the less this num is , the more dense object will be to one another).
    private Vector2 _v_size = Vector2.one * _size; // size in Vector2.

    private Stopwatch _enemyResTim = new Stopwatch();

    private Stopwatch _chestSpawnTim = new Stopwatch();

    private OBJ_SIZE _currentObjSize;
    private OBJ_TAG _currentObjTag;
   

    //ENEMY STUFF :
    public static Stopwatch EnemyXpGainTimer { get; private set; } = new Stopwatch();
    public static float EnemyXpGainMill { get; private set; } = 1000;
    public static float EnemyXpGain { get; private set; } = 10;
    public static float TotalEnemyXpGain { get; private set; } = 0;

    public static float GlobalTotalMill = EnemyXpGainMill;
    //--


    // GENERAL USE METHODS!
    private bool IsPosOnEdgeOfMap(int x, int y) // returns true if the selected tile pos is on the edge of the map.
    { return x == 0 || y == 0 || x == _mapWidth - 1 || y == _mapHeight - 1; }
    private bool OutOfBounds(int x, int y) // returns true of the pos given is out of the terrain.
    { return x < 0 || y < 0 || x > (_mapWidth - 2) || y > (_mapHeight - 2); }
    //--

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
        Time.timeScale = 1;
    }

    private void Start()
    {
        Player = GameObject.Find("Player");

        _chestSpawnTim.Start();

        _mapSolidsArr = new bool[_mapWidth, _mapHeight];
        // GENERATING RING OB BORDERS:
        GenerateBorders();

        // GENERATING MAP OBJECTS :
        GenerateSomething(OBJ_TAG.MAP_OBJ, OBJ_SIZE.SMALL ,  _mapSmallObjAmount);
        GenerateSomething(OBJ_TAG.MAP_OBJ, OBJ_SIZE.MED, _mapMedObjAmount);
        GenerateSomething(OBJ_TAG.MAP_OBJ, OBJ_SIZE.LARGE, _mapLargeObjAmount);

        _enemyResTim.Start();
        EnemyXpGainTimer.Start();
    }

    private void FixedUpdate()
    {
        if(_chestSpawnTim.ElapsedMilliseconds >= _treasureChestSpawnMill)
        {
            _chestSpawnTim.Restart();
            GenerateSomething(OBJ_TAG.CHEST , OBJ_SIZE.SMALL , 1 );
        }
        if (_enemyResTim.ElapsedMilliseconds >= _enemyResMill)
        {
            GenerateSomething(OBJ_TAG.UNIT, OBJ_SIZE.SMALL , _enemyAmount ,
                _enemyMinSpawnDistance);

            _enemyResTim.Restart();
        }
        if(EnemyXpGainTimer.ElapsedMilliseconds >= GlobalTotalMill)
        {
            enemyXpUpdate();
            if(_enemyResMill > _minEnemyResMill) { _enemyResMill -= _enemyResMillReduc; }
            else { _enemyResMill = _minEnemyResMill;  }
        }
    }

    private void enemyXpUpdate()
    {
        TotalEnemyXpGain += EnemyXpGain;
        GlobalTotalMill += EnemyXpGainMill;
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

    private void GenerateSomething(OBJ_TAG objTag , OBJ_SIZE objSize , int Amount , int minDistance = 0)
    {
        _currentObjSize = objSize;
        _currentObjTag = objTag;
        bool IsSolid = true;
        if(objTag == OBJ_TAG.UNIT) { IsSolid = false; }

        for (int i = 0; i < Amount; i++)
        {
            ApplyRandomPos();

            float dist = Mathf.Abs(Vector2.Distance(new Vector2(_rndX * _size, _rndY * _size * -1), Player.transform.position));
            if (minDistance != 0)
            {
                // to make the spawn quicker:
                int maxDistance = _enemyMaxSpawnDistance;
                int tries = 0;
                while (dist < minDistance || dist > maxDistance)
                {
                    tries++;
                    if(tries % 10 == 0)
                    {
                        maxDistance++;
                    }
                    ApplyRandomPos();
                    dist = Mathf.Abs(Vector2.Distance(new Vector2(_rndX * _size, _rndY * _size * -1), Player.transform.position));
                }
            }   
            
            GameObject randomPrfb = GetRandomPrefab(objTag , objSize);

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
        List<Vector2> v2List = new List<Vector2>(ProcGen.PrefabSizeDic[_currentObjSize][randomPrfb.name]);
        int prefabSize = v2List.Count;
        Vector2 v2 = ProcGen.PrefabSizeDic[_currentObjSize][randomPrfb.name].ToArray()[0];
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

    private void AddPrefabToMap(GameObject randomPrfb , bool IsSolid = true )
    {
        List<Vector2> v2List = new List<Vector2>(ProcGen.PrefabSizeDic[_currentObjSize][randomPrfb.name]);
     
        foreach(Vector2 v2 in v2List)
        {
            _mapSolidsArr[(int)v2.x + _rndX, (int)v2.y + _rndY] = IsSolid;
        }

        Vector2 pos = (v2List.ToArray()[0] + new Vector2(_rndX, _rndY)) * (_v_size * new Vector2(1 , -1));
        SpriteRenderer sprite = randomPrfb.GetComponent<SpriteRenderer>();
        if (_currentObjTag == OBJ_TAG.UNIT)
        {
            sprite.sortingOrder = 10;
        }
        else
        {
            sprite.sortingOrder = ((int)pos.y * -1) + 10;
        }
        Instantiate(randomPrfb, pos, Quaternion.identity);
    }


    // gets an gameObj instan of a random prefab from the dic pool:
    private GameObject GetRandomPrefab(OBJ_TAG objTag , OBJ_SIZE objSize)
    {
        int rnd = Random.Range(0, ProcGen.PrefabFetcherDic[objTag].Keys.Count);
        string str = ProcGen.PrefabFetcherDic[objTag].Keys.ToArray()[rnd];
        while(!ProcGen.PrefabSizeDic[objSize].Keys.Contains(str))
        {
            rnd = Random.Range(0, ProcGen.PrefabFetcherDic[objTag].Keys.Count);
            str = ProcGen.PrefabFetcherDic[objTag].Keys.ToArray()[rnd];
        }

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

                     {
                        "Rock" ,
                        Resources.Load("Prefabs/Environment/Rock", typeof(GameObject)) as GameObject
                     },

                     {
                        "Bush" ,
                        Resources.Load("Prefabs/Environment/Bush", typeof(GameObject)) as GameObject
                     },

                     {
                        "Trunk" ,
                        Resources.Load("Prefabs/Environment/Trunk", typeof(GameObject)) as GameObject
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

         {
            OBJ_TAG.CHEST, new Dictionary<string, GameObject>
            {
                {
                    "TreasureChest",
                    Resources.Load("Prefabs/Chest/TreasureChest", typeof(GameObject)) as GameObject
                },

                {
                    "TreasureChestClosed",
                    Resources.Load("Prefabs/Chest/TreasureChestClosed", typeof(GameObject)) as GameObject
                },
            }
        },

    };

    public static readonly Dictionary<OBJ_SIZE, Dictionary<string, List<Vector2>>> PrefabSizeDic = new Dictionary<OBJ_SIZE, Dictionary<string, List<Vector2>>>
    {
        {
            OBJ_SIZE.SMALL , new Dictionary<string, List<Vector2>>
            {
                 {
                    "Enemy-Default",
                     new List<Vector2>
                     {
                         new Vector2(0,0),
                     }
                 },


                {
                    "Border-Default",
                    new List<Vector2>
                    {
                        new Vector2(0,0),
                    }
                },

                {
                    "Rock",
                    new List<Vector2>
                    {
                        new Vector2(0,0),
                    }
                },

                {
                    "Bush",
                    new List<Vector2>
                    {
                        new Vector2(0,0),
                    }
                },

                {
                    "Trunk",
                    new List<Vector2>
                    {
                        new Vector2(0,0),
                    }
                },

                {
                    "TreasureChest",
                    new List<Vector2>
                    {
                        new Vector2(0,0),
                    }
                },

            }

        },

        {
            OBJ_SIZE.MED,new Dictionary<string, List<Vector2>>
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
                        new Vector2(1 , 0),
                    }
                },
            }
        },

        {
            OBJ_SIZE.LARGE,new Dictionary<string, List<Vector2>>
            {
                {
                    "Rock-monument" ,
                    new List<Vector2>
                    {
                          new Vector2(0 , 0),
                          new Vector2(1 , 0),
                          new Vector2(2 , 0),
                          new Vector2(0 , 1),
                          new Vector2(1 , 1),
                          new Vector2(2 , 1),
                    }
                },
            }
        },

    };



}


public enum OBJ_TAG
{
    MAP_OBJ,
    UNIT,
    BORDER,
    CHEST,
}

public enum OBJ_SIZE
{
    SMALL,
    MED,
    LARGE,
}
