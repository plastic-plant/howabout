using Howabout.Models;

namespace Howabout.Repositories
{
    public interface ISystemRepository
	{
		Task<SystemInfo> GetMetricsAsync();
	}
}
