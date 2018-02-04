namespace DifferentMethods.Extensions.Collections.Relational
{
    public interface ITableRow
    {
        int Id { get; set; }
        int RowVersion { get; set; }
        SerializedDatabase DB { get; set; }
    }
}