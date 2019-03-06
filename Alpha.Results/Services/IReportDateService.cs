namespace Alpha.Results.Site.Services
{
    using System;
    using Models.Dtos;

    public interface IReportDateService
    {
        DateTime GetBalanceUpDate();

        DateTime GetMoveBetsDate();

        BalanceUpAndMoveBetsDate GetBalanceUpAndMoveBetsDate();
    }
}