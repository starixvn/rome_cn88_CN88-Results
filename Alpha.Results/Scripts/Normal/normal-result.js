/// <reference path="../jquery-1.11.3.intellisense.js" />

"use strict";

var normalResult =
{
    resultDetailsUrl: 'Normal/ResultDetails?',
    excelResultsUrl: 'Normal/ExcelResults?',
    isIe: false,
    domElements:
        {
            txtResultDate: null,
            btnOtherDay: null,
            txtToday: null,
            txtMinResultDate: null,
            dpkContainer: null,
            resultForm: null,
            mainResult: null,
            rootUrl: null,
            resultDetails: null,
            excelBtn: null,
            ddlSports: null,
            ddlLeagues: null,
            filterForm: null,
            frmDetailsResult: null,
            resultDetailTitle: null,
            loadingImage: null
        },
    init: function () {

        normalResult.cacheElements();
        normalResult.registerEvents();

        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        if (msie > 0)// If Internet Explorer, return version number
        {
            normalResult.isIe = parseInt(ua.substring(msie + 5, ua.indexOf(".", msie))) < 10;
        }

    },
    cacheElements: function () {
        normalResult.domElements.txtResultDate = $('#ResultDate');
        normalResult.domElements.btnOtherDay = $('.btn-other-day');
        normalResult.domElements.txtMinResultDate = $('#MinResultDate');
        normalResult.domElements.dpkContainer = $('#dpkContainer');
        normalResult.domElements.mainResult = $('#main-result');
        normalResult.domElements.rootUrl = $('#RootURL');
        normalResult.domElements.resultDetails = $('#result-details');
        normalResult.domElements.excelBtn = $('#excel-btn');
        normalResult.domElements.ddlSports = $('#SportId');
        normalResult.domElements.ddlLeagues = $('#LeagueId');
        normalResult.domElements.filterForm = $('#result-filter');
        normalResult.domElements.frmDetailsResult = $('#result-detail-frm');
        normalResult.domElements.txtToday = $('#Today');
        normalResult.domElements.resultDetailTitle = $('#result-detailt-title');
        normalResult.domElements.loadingImage = $('#loading-image');
    },
    registerEvents: function () {
        normalResult.domElements.dpkContainer.datepicker({
            minDate: normalResult.domElements.txtMinResultDate.val(),
            allowInputToggle: true
        }).data("DateTimePicker").maxDate(normalResult.domElements.txtToday.val());

        normalResult.domElements.ddlSports.select2();
        normalResult.domElements.ddlLeagues.select2();

        normalResult.domElements.btnOtherDay
            .off('click.normalResult')
            .on('click.normalResult', function () {
                var $this = $(this);

                normalResult.domElements.dpkContainer.data("DateTimePicker").date($this.attr('value'));
            });

        normalResult.domElements.ddlSports
            .off('change.normalResult')
            .on('change.normalResult', function () {
                normalResult.domElements.ddlLeagues.val(normalResult.domElements.ddlLeagues.find('option:first').val());
                normalResult.domElements.filterForm.submit();
            });

        normalResult.domElements.resultDetails
            .off('hidden.bs.modal')
            .on('hidden.bs.modal', function () {
                normalResult.domElements.frmDetailsResult.hide();
                normalResult.domElements.loadingImage.show();
            });

        normalResult.domElements.mainResult
            .off('click.normalResult', '.detail-link')
            .on('click.normalResult', '.detail-link', function () {
                var $this = $(this),
                    requestParams = {
                        matchId: $this.data('match-id'),
                        leagueId: $this.data('league-id'),
                        sportId: $('#SportId').val()
                    };

                var resultDetailsUrl = normalResult.domElements.rootUrl.val()
                    + normalResult.resultDetailsUrl
                    + $.param(requestParams);

                normalResult.domElements.frmDetailsResult.attr('src', resultDetailsUrl);
                normalResult.domElements.resultDetails.modal({});
            });

        normalResult.domElements.frmDetailsResult
            .off('load.normalResult')
            .on('load.normalResult', function () {
                normalResult.domElements.loadingImage.hide();
                var $this = $(this);
                var detailTitle = $this.contents().find("title").html();
                normalResult.domElements.resultDetailTitle.html(detailTitle);

                $this.show();

                if (normalResult.isIe) {
                    var bodyWidth = $('body').width();
                    $this.width(bodyWidth - 200);
                    normalResult.domElements.resultDetails.modal({});
                    normalResult.domElements.resultDetails.find('.modal-dialog').width(bodyWidth - 100);
                }
        });

        normalResult.domElements.excelBtn
            .off('click.normalResult')
            .on('click.normalResult', function () {
                var requestParams = {
                    resultDate: normalResult.domElements.txtResultDate.val(),
                    leagueId: $('#LeagueId').val(),
                    sportId: $('#SportId').val()
                },
                downloadUrl = normalResult.domElements.rootUrl.val()
                    + normalResult.excelResultsUrl
                    + $.param(requestParams);

                var iframe = $('body #iframeExportExcelFile');
                if (iframe.length === 0) {
                    iframe = $('<iframe id="iframeExportExcelFile" width="0" height="0" class="display-none"></iframe>');
                    iframe.appendTo('body');
                }

                iframe.attr('src', downloadUrl);
            });
    }
};

$(function () {
    normalResult.init();
})