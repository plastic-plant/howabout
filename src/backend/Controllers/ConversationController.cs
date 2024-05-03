using Howabout.Interfaces;
using Howabout.Models;
using Microsoft.AspNetCore.Mvc;

namespace Howabout.Controllers
{
	public class ConversationController : Controller
	{
		private readonly ILogger<DocumentController> _logger;
		private readonly IConversationService _conversation;

		public ConversationController(ILogger<DocumentController> logger, IConversationService conversation)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_conversation = conversation ?? throw new ArgumentNullException(nameof(conversation));
        }
	}
}
