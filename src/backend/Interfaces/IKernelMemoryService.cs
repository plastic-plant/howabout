using Microsoft.KernelMemory;

namespace Howabout.Interfaces
{
	public interface IKernelMemoryService
	{
		public IKernelMemory? Get();
		public Task<bool> IsReadyAsync();
		public void Configure();
	}
}
