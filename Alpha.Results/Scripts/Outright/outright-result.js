(function () {
    'use strict';

    var rootUrl = $('#RootURL').val();
    var minResultDate = $('#MinResultDate').val(),
        maxResultDate = $('#MaxResultDate').val();
    var fromdatepicker, todatepicker;
    var sportTypeSelectbox, leagueSelectbox;

    function initializeDatepicker() {
        var dpkSettings = {
            minDate: minResultDate,
            allowInputToggle: true
        };

        fromdatepicker = $('#dpk-fromdate').datepicker(dpkSettings);
        fromdatepicker.data("DateTimePicker").maxDate(maxResultDate);
        todatepicker = $('#dpk-todate').datepicker(dpkSettings);
        todatepicker.data("DateTimePicker").maxDate(maxResultDate);

        fromdatepicker.on('dp.change', function (e) {
            todatepicker.data('DateTimePicker').minDate(e.date);
        });

        todatepicker.on('dp.change', function (e) {
            fromdatepicker.data('DateTimePicker').maxDate(e.date);
        });
    }

    function initializeSelectbox() {
        sportTypeSelectbox = $('#SportType').select2();
        leagueSelectbox = $('#LeagueId').select2();
    }

    function registerEvents() {
        $('.btn-other-day').click(function () {
            var date = $(this).attr('value');
            fromdatepicker.data('DateTimePicker').date(date);
            todatepicker.data('DateTimePicker').date(date);
        });

        $('#btn-excel').click(function () {
            var requestParams = {
                fromDate: fromdatepicker.data('date'),
                toDate: todatepicker.data('date'),
                sportType: sportTypeSelectbox.val(),
                leagueId: leagueSelectbox.val()
            };

            var downloadUrl = rootUrl + 'Outright/ExcelResults?' + $.param(requestParams);

            var iframe = $('#ifrm-exportExcel');
            if (iframe.length === 0) {
                iframe = $('<iframe id="ifrm-exportExcel" width="0" height="0" class="display-none"></iframe>');
                iframe.appendTo('body');
            }

            iframe.attr('src', downloadUrl);
        });
    }

    function initialize() {
        initializeDatepicker();
        initializeSelectbox();
        registerEvents();
    }

    initialize();
})();
