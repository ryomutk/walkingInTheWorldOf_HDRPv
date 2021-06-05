using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility;
using Utility.ObjPool;
using ModulePattern;

using World.Wrapper;
using World.Building;
using World.Data.Geometry;
using World.Data.Database;
using World.Map;



namespace World.Builder
{
    /// <summary>
    /// 生成した建造物の場所と範囲を記録する。
    /// 誰かが動的に生成して、インターフェイスを用意する想定
    /// </summary>
    public class SquareBuilder : Singleton<SquareBuilder>
    {

        [SerializeField] Square squarePref;
        [SerializeField] int reqSqNum = 800;
        [SerializeField] int reqStrNum = 300;
        ModuleState _state;
        InstantPool<Square> squarePool;
        List<InstantPool<Structure>> strPools = new List<InstantPool<Structure>>();
        SquareMapper mapper;

        SquareMetaData metaSqr = new SquareMetaData();

        protected override void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            mapper = SquareMapper.instance;
            StartCoroutine(CreatePool());
        }

        IEnumerator CreatePool()
        {
            squarePool = new InstantPool<Square>();
            squarePool.CreatePool(squarePref, reqSqNum);

            yield return new WaitUntil(() => squarePool.state == ModuleState.ready);
            LogWriter.Log("SQUARE POOL INITIALIZED", "SquareLog");

            for (var i = 0; i < StructureDB.instance.length; i++)
            {
                var pool = new InstantPool<Structure>();
                pool.CreatePool(StructureDB.instance.GetStructure(i), reqStrNum);
                yield return new WaitUntil(() => squarePool.state == ModuleState.ready);
                strPools.Add(pool);
            }
            Debug.Log("initialized!");
            _state = ModuleState.ready;
        }

        [Sirenix.OdinInspector.Button]
        public bool Build(int id, Vector3Int coordinate, Vector3Int rotationEuler, bool setRandom = false)
        {
            if (_state != ModuleState.ready)
            {
                return false;
            }

            var str = StructureDB.instance.GetStructure(id);

            return Build(str, coordinate, rotationEuler, setRandom);
        }

        public bool Build(string name, Vector3Int coordinate, Vector3Int rotationEuler, bool setRandom = false)
        {
            if (_state != ModuleState.ready)
            {
                return false;
            }

            var str = StructureDB.instance.GetStructure(name);

            return Build(str, coordinate, rotationEuler, setRandom);
        }

        public bool Build(Structure structurePref, Vector3Int coordinate, Vector3Int rotationEuler, bool setRandom = false)
        {
            if (_state != ModuleState.ready)
            {
                Debug.LogWarning("not ready!");
                return false;
            }

            if (!structurePref.mapped)
            {
                structurePref.Remap();
            }

            metaSqr.MapSquare(structurePref, coordinate, rotationEuler);

            if (!mapper.CheckSquare(metaSqr))
            {
                return false;
            }

            _state = ModuleState.working;

            var sqr = squarePool.GetObj();

            var strSize = structurePref.size;

            int id = StructureDB.instance.GetId(structurePref);

            var strInstance = strPools[id].GetObj();

            LogWriter.Log("SQUARE:" + sqr.name + " BUILT. " + " USED:" + strInstance.name, "SquareLog");

            sqr.SetStructure(strInstance, coordinate, rotationEuler, setRandom);

            mapper.AddSquare(sqr);

            _state = ModuleState.ready;

            return true;
        }
    }
}