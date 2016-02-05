using System;
using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using MassTransit;

namespace Messy.MassTransitIntegration.Extensions
{
    public static class StateMachineExtensions
    {
        public static Task RaiseEventWithContext<T, TInstance, TData>(
            this T machine,
            TInstance instance,
            Event eventSelector,
            BehaviorContext<TInstance, TData> behaviorContext,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : MassTransitStateMachine<TInstance> where TInstance : class, SagaStateMachineInstance
            where TData : class
        {
            ConsumeContext consumeContext;
            behaviorContext.TryGetPayload(out consumeContext);

            return machine.RaiseEvent(instance, eventSelector, consumeContext, cancellationToken);
        }

        public static Task RaiseEventWithContext<T, TInstance>(
            this T machine,
            TInstance instance,
            Event eventSelector,
            BehaviorContext<TInstance> behaviorContext,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : MassTransitStateMachine<TInstance> where TInstance : class, SagaStateMachineInstance
        {
            ConsumeContext consumeContext;
            behaviorContext.TryGetPayload(out consumeContext);

            return machine.RaiseEvent(instance, eventSelector, consumeContext, cancellationToken);
        }

        public static ConsumeContext GetConsumeContext<T>(this BehaviorContext<T> behaviorContext)
        {
            ConsumeContext consumeContext;
            if (!behaviorContext.TryGetPayload(out consumeContext))
            {
                throw new Exception();
            }

            return consumeContext;
        }
    }
}