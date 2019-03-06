namespace Alpha.Results.Site.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using Models.Views;
    using Web;

    public class OutrightController : BaseController
    {
        private readonly IMediator _mediator;

        public OutrightController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [HttpGet]
        public ActionResult Index(OutrightResultQuery request = null)
        {
            request = request ?? new OutrightResultQuery();

            var viewModel = _mediator.Send(request);

            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [HttpGet]
        public ActionResult ExcelResults(ExcelOutrightResultQuery request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null");
            }

            request.TemplatePath = Helpers.UrlHelper.ResultTemplatePath;

            var viewModel = _mediator.Send<OutrightResultExcelViewModel>(request);

            return File(viewModel.ExcelResults, "application/vnd.ms-excel", viewModel.ExcelName);
        }
    }
}
