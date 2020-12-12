$(function () {
  try {
    var dir = $('#dir').val();
    var col = $('#col').val();
    var header = $("th a[href*=" + col + "]");
    if (dir == "Ascending") {
      header.text(header.text() + "  ▲")
    }
    if (dir == "Descending") {
      header.text(header.text() + "  ▼");
    }
  } catch (e) { }
})