$(function () {
  $('.chkactif').change(function () {
    var self = $(this);
    var id = self.attr('value');
    var value = self.prop('checked');

    $.ajax({
      url: 'Flux/UpdateActif',
      data: { id: id, isActif: value },
      type: 'POST'
      //success: function (data) { alert('success'); },
      //error: function () { alert('error'); }
    });
  });
});