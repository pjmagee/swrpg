using Discord.Interactions;

using Orleans.Runtime;

namespace SWRPG.Discord.Interactions.Common;

public abstract class OrleansInteractionModuleBase<T> : InteractionModuleBase<T> where T : SocketInteractionContext
{
    public override Task BeforeExecuteAsync(ICommandInfo command)
    {
        RequestContext.Set("Tenant", Context.Guild.Id);
        return base.BeforeExecuteAsync(command);
    }

    public override void AfterExecute(ICommandInfo command)
    {
        RequestContext.Remove("Tenant");
        base.AfterExecute(command);
    }

    public override Task AfterExecuteAsync(ICommandInfo command)
    {
        RequestContext.Remove("Tenant");
        return base.AfterExecuteAsync(command);
    }

    public override void BeforeExecute(ICommandInfo command)
    {
        RequestContext.Set("Tenant", Context.Guild.Id);
        base.BeforeExecute(command);
    }
}