using System.Diagnostics.CodeAnalysis;

namespace GZip.Logic.Logic.TaskManagement;

/// <summary> Working queue for tasks to be processed </summary>
public interface ITaskQueue<T>
{
    /// <summary> Returns true if queue no empty yet </summary>
    public bool NotEmpty();

    /// <summary> Try to get element and delete it from queue </summary>
    /// <param name="result"> Element </param>
    public void TryDequeue([MaybeNullWhen(false)] out T result);

    /// <summary> Put element into queue </summary>
    /// <param name="item"> Element </param>
    public void Enqueue(T item);
}