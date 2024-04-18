using Microsoft.KernelMemory;

namespace Howabout.Interfaces
{
	public interface IKernelMemoryService
	{
		public IKernelMemory? Get();
		public void Configure();
	}
}
