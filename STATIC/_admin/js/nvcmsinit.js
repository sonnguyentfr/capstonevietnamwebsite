function notifySuccess(str) {
    $('#divMessage').html(str);
    $('#imgInfo').attr('src', '/images/data_ok.png');
    $('.MsgBox').fadeIn('slow');
    $('img.close').click(function () {
        $('.MsgBox').fadeOut('slow');
    });

    setTimeout(function () {
        $('.MsgBox').fadeOut('slow');
    }, 4000);
    return false;
}
function notifyError(str) {
    $('#divMessage').html(str);
    $('#imgInfo').attr('src', '/images/server_warning.png');
    $('.MsgBox').fadeIn('slow');
    $('img.close').click(function () {
        $('.MsgBox').fadeOut('slow');
    });

    setTimeout(function () {
        $('.MsgBox').fadeOut('slow');
    }, 4000);
    return false;
}
function readURL(input) {
    if (input.files.length > 0) {
        document.getElementById('soluongileax').innerHTML = 'Đã chọn: <span class="filechon">' + input.files.length + '</span> files</br >';
    }
}
function getExt(filename) {
    var ext = filename.split('.').pop();
    if (ext == filename) return "";
    return ext.toLowerCase();
}

function OpenDialogHistory() {
    $("#modal-history").modal();
}
function OpenDialogNewsNotes() {
    $("#modal-newsnote").modal();
}
function OpenDialogNewsNotesSubmit() {
    $("#modal-newsnote-submit").modal();
}
function OpenDialogNewsNotesSubmit2() {
    $("#modal-newsnote-submit2").modal();
}
function OpenDialogNewsNotesSubmit3() {
    $("#modal-newsnote-submit3").modal();
}
function NewsNotesToast(loinhan) {
    toastr.clear();
    NioApp.Toast(loinhan, 'info', {
        position: 'top-right',
        timeOut: '6000000',
        extendedTimeOut: "300000"
    });
}
function OpenDialogSuaNgayXuatBan() {
    $("#modal-editngayxuatban").modal();
}
function CloseDialogSuaNgayXuatBan() {
    //$("#modal-editngayxuatban").modal('hide');
    $("#modal-editngayxuatban").removeClass("in");
    $(".modal-backdrop").remove();
    $('body').removeClass('modal-open');
    $('body').css('padding-right', '');
    $("#modal-editngayxuatban").hide();
}
function UpdateSuccess(title) {
    Swal.fire({
        position: 'top-end',
        icon: 'success',
        title: title,
        showConfirmButton: false,
        timer: 1500
    });
}
function UpdateError(title) {
    Swal.fire(title, "Click vào OK để thoát", "error");
}
//Mr Doi them
$(document).ready(function () {
    $('.datetimepicker').datetimepicker({
        format: 'd/m/Y H:i',
        formatDate: 'd/m/Y H:i',
        step: 3,
        allowInputToggle: true
    });
    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        allowInputToggle: true
    });
    $('.auto').autoNumeric('init', { dGroup: '3', aSep: '.', aDec: ',', aSign: '', vMin: '0', vMax: '10000000', wEmpty: 'zero', wEmpty: 'sign' });
});
