namespace DataMigrator.Common.Models;

public interface ITransform
{
    object Transform(object value);
}