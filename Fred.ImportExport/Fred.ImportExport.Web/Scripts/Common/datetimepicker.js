$(function () {
    $(document).ready(function () {
        $("#fdate").datetimepicker({
            dateFormat: 'dd/mm/yy',
            timeFormat: 'HH:mm:ss',
            lang: 'fr', //marche pas
            onShow: function () {
                this.setOptions({
                    maxDate: $('#tdate').val() ? $('#tdate').val() : false,
                    maxTime: $('#tdate').val() ? $('#tdate').val() : false
                });
            }
        });
        $("#tdate").datetimepicker({
            dateFormat: 'dd/mm/yy',
            timeFormat: 'HH:mm:ss',
            lang: 'fr', //marche pas
            onShow: function () {
                this.setOptions({
                    minDate: $('#fdate').val() ? $('#fdate').val() : false,
                    minTime: $('#fdate').val() ? $('#fdate').val() : false
                });
            }
        });

    });
});