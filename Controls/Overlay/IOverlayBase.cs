using Microsoft.AspNetCore.Components;

namespace DevBuddy.Controls.Overlay;

public interface IOverlayBase : IComponent
{
    public Guid InstanceId { get; }
}
