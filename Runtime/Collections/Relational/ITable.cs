namespace DifferentMethods.Extensions.Collections.Relational
{
    public interface ITable
    {
        bool Commit();
        void Abort();
        bool Verify();
        void OnEnable();
        bool IsDirty { get; }
        int[] Keys { get; }
        ITableRow this[int key] { get; }
        SerializedDatabase database { get; set; }
    }
}