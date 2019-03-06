using Alpha.Results.Site.Models.Views;
using MediatR;

namespace Alpha.Results.Site.Models.Requests
{
    public class LotteryResultQuery : IRequest<LotteryResultViewModel>
    {
        public int CustId { get; set; }

        public int CustLevelId { get; set; }

        public string LanguageKey { get; set; }

        public string RootPath { get; set; }
    }
}