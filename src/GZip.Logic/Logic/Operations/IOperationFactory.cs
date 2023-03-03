namespace GZip.Logic.Logic.Operations;

/// <summary> Creates operation implementation dependent upon type </summary>
public interface IOperationFactory
{
    /// <summary> Creates operation implementation dependent upon type </summary>
    /// <param name="operationType"> Type of operation to create </param>
    /// <returns> Concrete operation implementation </returns>
    IOperation Create(OperationType operationType);
}