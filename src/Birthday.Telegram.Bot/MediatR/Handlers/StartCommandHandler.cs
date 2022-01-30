using Birthday.Telegram.Bot.MediatR.Commands;
using MediatR;

namespace Birthday.Telegram.Bot.MediatR.Handlers
{
    /// <summary>
    /// Handler for processing start command
    /// </summary>
    public class StartCommandHandler : IRequestHandler<StartCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StartCommandHandler> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator">IMediatR instance</param>
        /// <param name="logger">ILogger instance</param>
        public StartCommandHandler(IMediator mediator, ILogger<StartCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Handle of request
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="cancellationToken">Instance of cancellation token</param>
        public Task<Unit> Handle(StartCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}