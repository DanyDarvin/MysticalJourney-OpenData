namespace Common.Pools.Abstract
{
    public interface IPool<in TObjectTypeId, TPooledObject> where TPooledObject : class
    {
        void CreatePool(TObjectTypeId objectTypeId);
        TPooledObject TryGetObject(TObjectTypeId objectTypeId);
        void Release(TObjectTypeId objectTypeId, TPooledObject pooledObject);
        void Cleanup();
    }
}