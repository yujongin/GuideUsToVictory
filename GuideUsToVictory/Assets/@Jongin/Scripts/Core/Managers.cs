using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    private static Managers Instance { get {  return instance; } }
    private MapManager _map;

    private GameManager _game;
    private PoolManager _pool;
    public static MapManager Map { get { return Instance?._map; } }
    public static GameManager Game { get { return Instance ?. _game; } }
    public static PoolManager Pool {  get { return Instance?._pool; } } 

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
        _pool = new PoolManager();
    }
}
