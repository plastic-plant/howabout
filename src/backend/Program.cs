
using Howabout.Configuration;
using Howabout.Interfaces;
using Howabout.Services;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.Configuration;

namespace Howabout
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddSingleton<IKernelMemoryService, KernelMemoryService>();
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseSwagger();
			app.UseSwaggerUI();
			app.MapControllers();
			app.MapFallbackToFile("/index.html");
			await app.RunAsync();
		}
	}
}
