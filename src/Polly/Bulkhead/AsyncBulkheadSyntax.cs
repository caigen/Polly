#nullable enable
namespace Polly;

#pragma warning disable CA1724
public partial class Policy
#pragma warning restore CA1724
{
    /// <summary>
    /// <para>Builds a bulkhead isolation <see cref="Policy"/>, which limits the maximum concurrency of actions executed through the policy.  Imposing a maximum concurrency limits the potential of governed actions, when faulting, to bring down the system.</para>
    /// <para>When an execution would cause the number of actions executing concurrently through the policy to exceed <paramref name="maxParallelization"/>, the action is not executed and a <see cref="BulkheadRejectedException"/> is thrown.</para>
    /// </summary>
    /// <param name="maxParallelization">The maximum number of concurrent actions that may be executing through the policy.</param>
    /// <returns>The policy instance.</returns>
    public static AsyncBulkheadPolicy BulkheadAsync(int maxParallelization)
    {
        Func<Context, Task> doNothingAsync = _ => TaskHelper.EmptyTask;
        return BulkheadAsync(maxParallelization, 0, doNothingAsync);
    }

    /// <summary>
    /// <para>Builds a bulkhead isolation <see cref="Policy"/>, which limits the maximum concurrency of actions executed through the policy.  Imposing a maximum concurrency limits the potential of governed actions, when faulting, to bring down the system.</para>
    /// <para>When an execution would cause the number of actions executing concurrently through the policy to exceed <paramref name="maxParallelization"/>, the action is not executed and a <see cref="BulkheadRejectedException"/> is thrown.</para>
    /// </summary>
    /// <param name="maxParallelization">The maximum number of concurrent actions that may be executing through the policy.</param>
    /// <param name="onBulkheadRejectedAsync">An action to call asynchronously, if the bulkhead rejects execution due to oversubscription.</param>
    /// <returns>The policy instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">maxParallelization;Value must be greater than zero.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="onBulkheadRejectedAsync"/> is <see langword="null"/>.</exception>
    public static AsyncBulkheadPolicy BulkheadAsync(int maxParallelization, Func<Context, Task> onBulkheadRejectedAsync) =>
        BulkheadAsync(maxParallelization, 0, onBulkheadRejectedAsync);

    /// <summary>
    /// Builds a bulkhead isolation <see cref="Policy" />, which limits the maximum concurrency of actions executed through the policy.  Imposing a maximum concurrency limits the potential of governed actions, when faulting, to bring down the system.
    /// <para>When an execution would cause the number of actions executing concurrently through the policy to exceed <paramref name="maxParallelization" />, the policy allows a further <paramref name="maxQueuingActions" /> executions to queue, waiting for a concurrent execution slot.  When an execution would cause the number of queuing actions to exceed <paramref name="maxQueuingActions" />, a <see cref="BulkheadRejectedException" /> is thrown.</para>
    /// </summary>
    /// <param name="maxParallelization">The maximum number of concurrent actions that may be executing through the policy.</param>
    /// <param name="maxQueuingActions">The maximum number of actions that may be queuing, waiting for an execution slot.</param>
    /// <returns>The policy instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">maxParallelization;Value must be greater than zero.</exception>
    /// <exception cref="ArgumentOutOfRangeException">maxQueuingActions;Value must be greater than or equal to zero.</exception>
    public static AsyncBulkheadPolicy BulkheadAsync(int maxParallelization, int maxQueuingActions)
    {
        Func<Context, Task> doNothingAsync = _ => TaskHelper.EmptyTask;
        return BulkheadAsync(maxParallelization, maxQueuingActions, doNothingAsync);
    }

    /// <summary>
    /// Builds a bulkhead isolation <see cref="Policy" />, which limits the maximum concurrency of actions executed through the policy.  Imposing a maximum concurrency limits the potential of governed actions, when faulting, to bring down the system.
    /// <para>When an execution would cause the number of actions executing concurrently through the policy to exceed <paramref name="maxParallelization" />, the policy allows a further <paramref name="maxQueuingActions" /> executions to queue, waiting for a concurrent execution slot.  When an execution would cause the number of queuing actions to exceed <paramref name="maxQueuingActions" />, a <see cref="BulkheadRejectedException" /> is thrown.</para>
    /// </summary>
    /// <param name="maxParallelization">The maximum number of concurrent actions that may be executing through the policy.</param>
    /// <param name="maxQueuingActions">The maximum number of actions that may be queuing, waiting for an execution slot.</param>
    /// <param name="onBulkheadRejectedAsync">An action to call asynchronously, if the bulkhead rejects execution due to oversubscription.</param>
    /// <returns>The policy instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">maxParallelization;Value must be greater than zero.</exception>
    /// <exception cref="ArgumentOutOfRangeException">maxQueuingActions;Value must be greater than or equal to zero.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="onBulkheadRejectedAsync"/> is <see langword="null"/>.</exception>
    public static AsyncBulkheadPolicy BulkheadAsync(
        int maxParallelization,
        int maxQueuingActions,
        Func<Context, Task> onBulkheadRejectedAsync)
    {
        if (maxParallelization <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxParallelization), "Value must be greater than zero.");
        }

        if (maxQueuingActions < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxQueuingActions), "Value must be greater than or equal to zero.");
        }

        if (onBulkheadRejectedAsync == null)
        {
            throw new ArgumentNullException(nameof(onBulkheadRejectedAsync));
        }

        return new AsyncBulkheadPolicy(
            maxParallelization,
            maxQueuingActions,
            onBulkheadRejectedAsync);
    }
}
