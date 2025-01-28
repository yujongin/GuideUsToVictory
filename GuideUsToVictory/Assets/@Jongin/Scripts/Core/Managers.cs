using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    private static Managers Instance { get {  return instance; } }
    private MapManager _map;

    private GameManager _game;
    private PoolManager _pool;
    private ResourceManager _resource;
    private UnitSpawnManager _unitSpawn;
    private UIManager _ui;
    private SummonGroundManager _summonGround;
    public static MapManager Map { get { return Instance?._map; } }
    public static GameManager Game { get { return Instance ?. _game; } }
    public static PoolManager Pool {  get { return Instance?._pool; } } 
    public static ResourceManager Resource {  get { return Instance?._resource; } } 
    public static UnitSpawnManager UnitSpawn {  get { return Instance?._unitSpawn; } } 
    public static UIManager UI { get { return Instance?._ui; } }
    public static SummonGroundManager SummonGround { get { return Instance?._summonGround; } }
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _map = GetComponentInChildren<MapManager>();
        _game = GetComponentInChildren<GameManager>();
        _resource = GetComponentInChildren<ResourceManager>();
        _unitSpawn = GetComponentInChildren<UnitSpawnManager>();
        _ui = GetComponentInChildren<UIManager>();
        _summonGround = GetComponentInChildren<SummonGroundManager>();
        _pool = new PoolManager();
    }
}
