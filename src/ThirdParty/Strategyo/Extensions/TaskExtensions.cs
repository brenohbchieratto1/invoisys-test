using System.Reflection;

namespace Strategyo.Extensions;

public static class TaskExtensions
{
    private static readonly MethodInfo InvokeAndAwaitMethodInfo =
        typeof(TaskExtensions).GetMethod(nameof(InvokeAndAwaitReflectedMethodImplAsync),
                                         BindingFlags.NonPublic | BindingFlags.Static)!;
    
    private static Task ForgetAwaited(Task task)
        => task;
    
    /// <summary>
    ///     Observes the task to avoid the UnobservedTaskException event to be raised.
    /// </summary>
    public static void Forget(this Task task)
    {
        if (!task.IsCompleted || task.IsFaulted)
        {
            _ = ForgetAwaited(task);
        }
    }
    
    public static void FireAndForget(this Task task)
        => Task.Run(async () => await task).Forget();
    
    public static async Task<object?> InvokeAndAwaitReflectedMethodAsync(object obj, MethodInfo methodInfo,
                                                                         object?[] args)
    {
        var methodTaskInnerType = methodInfo.ReturnType.GetGenericArguments()[0];
        
        var invokerGen = InvokeAndAwaitMethodInfo.MakeGenericMethod(obj.GetType(), methodTaskInnerType);
        
        var argsForInvokeAndAwait = new[]
        {
            obj, methodInfo, args
        };
        
        var asTaskObj = (Task<object?>)invokerGen.Invoke(null, argsForInvokeAndAwait)!;
        
        var result = await asTaskObj;
        return result;
    }
    
    private static async Task<object?> InvokeAndAwaitReflectedMethodImplAsync<TObject, TReturn>(TObject obj,
                                                                                                MethodInfo methodInfo, object?[] args)
    {
        var typeOfTaskOfT = typeof(Task<TReturn>);
        if (methodInfo.ReturnType != typeOfTaskOfT)
        {
            var msg = "ReturnType does not match T. Expected: " + typeOfTaskOfT.FullName + ", Actual: " +
                      methodInfo.ReturnType.FullName;
            throw new ArgumentException(msg, nameof(methodInfo));
        }
        
        var taskObj = methodInfo.Invoke(obj, args);
        switch (taskObj)
        {
            case null:
                throw new InvalidOperationException("Method returned null. Expected Task.");
            case Task<TReturn> task:
            {
                var value = await task.ConfigureAwait(false);
                object? valueAsObject = value;
                return valueAsObject;
            }
            default:
                throw new InvalidOperationException("Unexpected return value.");
        }
    }
}