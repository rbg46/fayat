$(function () {
  //$('.container').css({
  //  'position': 'absolute',
  //  'left': '50%',
  //  'margin-left': -$('.container').outerWidth() / 2
  //});
  var boutonActif = "none";

  $("#boutonTri").click(function () {
    if (boutonActif == "tri") {
      $("#demo").toggle(400);
      $("#c").toggle(400);
      boutonActif = "none";
    }
    else {
      $("#c").toggle(400);
      if (boutonActif == "none") {
        $("#demo").toggle(400);
      }
      else {
        $("#a:visible").toggle(400);
        $("#b:visible").toggle(400);
      }
      boutonActif = "tri";
    }
  });

  $("#boutonFiltre").click(function () {
    if (boutonActif == "filtre") {
      $("#demo").toggle(400);
      $("#b").toggle(400);
      boutonActif = "none";
    }
    else {
      $("#b").toggle(400);
      if (boutonActif == "none") {
        $("#demo").toggle(400);
      }
      else {
        $("#a:visible").toggle(400);
        $("#c:visible").toggle(400);
      }
      boutonActif = "filtre";
    }
  });

  $("#boutonRecherche").click(function () {
    if (boutonActif == "recherche") {
      $("#demo").toggle(400);
      $("#a").toggle(400);
      boutonActif = "none";
    }
    else {
      $("#a").toggle(400);
      if (boutonActif == "none") {
        $("#demo").toggle(400);
      }
      else {
        $("#b:visible").toggle(400);
        $("#c:visible").toggle(400);
      }
      boutonActif = "recherche";
    }
  });

  $("#demo").on("hide.bs.collapse", function () {
    $(".filter").html('<span class="fa fa-filter"></span>');
    $('.overlay').hide()
  });
  $("#demo").on("show.bs.collapse", function () {
    $(".filter").html('<span title="' + resources.Revenir_recherche_simple_lb + '" class="material-icons" style="color: rgb(165,165,165);font-size: 25px;display: flex;justify-content: center;align-items: center;">clear</span>');
    $('.overlay').show()
  });

  $('select').on('change', function (ev) {
    var selected = $(this).find('option:selected');
    $('#' + $(this).attr('id') + 'Text').text(selected.text());
  });
});