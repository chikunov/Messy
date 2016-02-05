using System;
using System.Threading.Tasks;
using Autofac;
using Messy.Messaging;
using NLog;

namespace Messy.DDD
{
    public abstract class BoundedContextBase
    {
        public string Prefix { get; }
        public string ContextName { get; }
        public string FullName { get; }

        protected IContainer Container { get; private set; }
        protected Logger Logger { get; }
        protected IMessageBus Bus { get; private set; }

        protected BoundedContextBase(string prefix, string contextName)
        {
            ContextName = contextName;
            Prefix = prefix;
            FullName = GetFullName(prefix, contextName);
            Logger = LogManager.GetLogger(contextName);
        }

        public static string GetFullName(string prefix, string contextName)
        {
            return $"{prefix}.{contextName}";
        }

        public void Start()
        {
            try
            {
                var cb = new ContainerBuilder();
                InitializeIoc(cb);
                Container = cb.Build();

                Bus = Container.Resolve<IMessageBus>();
                Bus.Start();
                Logger.Debug("Service started on queue {0}", Bus.GetAddress());

                AfterStart().Wait();
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Error on starting service");
                throw;
            }
        }

        public virtual Task AfterStart()
        {
            return Task.CompletedTask;
        }

        public abstract void InitializeIoc(ContainerBuilder builder);

        public void Stop()
        {
            try
            {
                Bus?.Dispose();
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Error on stopping service");
                throw;
            }

            Logger.Debug("Service stopped");

            AfterStop();
        }

        public virtual Task AfterStop()
        {
            return Task.CompletedTask;
        }
    }
}