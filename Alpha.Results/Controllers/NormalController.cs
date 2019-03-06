namespace Alpha.Results.Site.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using Models.Views;
    using Web;

    public class NormalController : BaseController
    {
        private readonly IMediator _mediator;

        public NormalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [HttpGet]
        public ActionResult Index(NormalResultQuery request = null)
        {
            request = request ?? new NormalResultQuery();

            var viewModel = _mediator.Send(request);

            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [HttpGet]
        public ActionResult ResultDetails(NormalResultDetailsQuery request)
        {
            var viewModel = _mediator.Send<NormalDetailViewModel>(request);

            return View(viewModel);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [HttpGet]
        public ActionResult ExcelResults(ExcelNormalResultQuery request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null");
            }

            request.TemplatePath = Helpers.UrlHelper.ResultTemplatePath;

            var viewModel = _mediator.Send<NormalResultExcelViewModel>(request);

            return File(viewModel.ExcelResults, "application/vnd.ms-excel", viewModel.ExcelName);
        }
    }
}
